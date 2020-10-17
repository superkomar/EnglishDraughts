using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Interfaces;
using Core.Models;
using Core.Properties;
using Core.Utils;

using NLog;

namespace Core
{
    public class GameController
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ModelController _modelController;
        private readonly PlayerStateMachine _playersControl;

        private bool _isGameRunning;
        private bool _isHistoryRolling;
        private CancellationTokenSource _tokenSource;

        public GameController(int dimension, IGamePlayer blackPlayer, IGamePlayer whitePlayer)
        {
            if (whitePlayer == null || blackPlayer == null)
            {
                throw new ArgumentNullException(Resources.Error_NullPlayers);
            }

            _modelController = new ModelController(dimension);
            _playersControl = new PlayerStateMachine(blackPlayer, whitePlayer);

            _isGameRunning = _isHistoryRolling = false;
        }

        #region IGameController

        public GameState? FinalGameState { get; private set; }

        public async IAsyncEnumerable<GameState> StartGameAsync()
        {
            Logger.Info(Resources.Info_GameStart);

            _playersControl.BlackPlayer.InitGame(PlayerSide.Black);
            _playersControl.WhitePlayer.InitGame(PlayerSide.White);

             yield return new GameState(_modelController.Field, StateType.Start, _playersControl.CurPlayer.Side);

            _isGameRunning = true;

            while (_isGameRunning)
            {
                yield return await MakeTurnAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public void FinishGame() => FinishGame(_playersControl.CurPlayer.Side.ToOpposite());

        public void Redo(int deep)
        {
            _modelController.Redo(deep);
            HistoryTraverser();
        }

        public void Undo(int deep)
        {
            _modelController.Undo(deep);
            HistoryTraverser();
        }

        #endregion

        private void HistoryTraverser()
        {
            Logger.Info(Resources.Info_HistoryRoll);

            _isHistoryRolling = true;
            _tokenSource?.Cancel();
            _playersControl.ChangeStateForNextGet(PlayerStateMachine.MachineState.Repeat);
        }

        private async Task<GameState> MakeTurnAsync()
        {
            _tokenSource = new CancellationTokenSource();

            var (Player, Side) = _playersControl.GetNextPlayer();

            // Get turn
            var newTurn = await Task.Run(() => Player.MakeTurn(_modelController.Field, _tokenSource.Token))
                .ConfigureAwait(continueOnCapturedContext: false);

            Logger.Info(newTurn);

            // Check if game stoped
            if (FinalGameState != null) return FinalGameState.Value;

            // Check if returned value the reason of history rolling
            if (!_isHistoryRolling)
            {
                // Check turn
                var success = TryUpdateField(newTurn);

                // Check end of game
                if (!success || GameRules.HasPlayerWon(_modelController.Field, Side))
                {
                    FinishGame(success ? Side : Side.ToOpposite());
                    return FinalGameState.Value;
                }
            }

            _isHistoryRolling = false;

            return new GameState(_modelController.Field, StateType.Turn, Side);
        }

        private void FinishGame(PlayerSide winner)
        {
            Logger.Info(Resources.Info_GameEnd);

            _isGameRunning = false;
            FinalGameState = new GameState(_modelController.Field, StateType.Finish, winner);

            _tokenSource?.Cancel();

            _playersControl.BlackPlayer.FinishGame(winner);
            _playersControl.WhitePlayer.FinishGame(winner);
        }

        private bool TryUpdateField(GameTurn newTurn)
        {
            if (newTurn == null ||
                !GameFieldUtils.TryCreateField(_modelController.Field, newTurn, out GameField newGameField))
            {
                return false;
            }

            _modelController.UpdateField(newGameField);

            return true;
        }
    }
}
