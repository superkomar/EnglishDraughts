using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

namespace Core
{
    public class GameController
    {
        private readonly ModelController _modelController;
        private readonly PlayerStateMachine _playersControl;

        private bool _isGameRunning;
        private bool _isHistoryRolling;

        public GameController(int dimension, IGamePlayer blackPlayer, IGamePlayer whitePlayer)
        {
            if (whitePlayer == null || blackPlayer == null) throw new ArgumentNullException();

            _modelController = new ModelController(dimension);
            _playersControl = new PlayerStateMachine(blackPlayer, whitePlayer);

            _isGameRunning = _isHistoryRolling = false;
        }

        #region IGameController

        public GameState? FinalGameState { get; private set; }

        public void Redo(int deep)
        {
            _modelController.Redo(deep);
            HistoryTraverser();
        }

        public async IAsyncEnumerable<GameState> StartGameAsync()
        {
            if (FinalGameState != null)
            {
                yield return FinalGameState.Value;
                yield break;
            }

            yield return new GameState(_modelController.Field, StateType.Start, _playersControl.CurPlayer.Side);

            _isGameRunning = true;

            while (_isGameRunning)
            {
                yield return await MakeTurnAsync()
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
        }

        public void StopGame()
        {
            if (_isHistoryRolling || FinalGameState == null)
            {
                _isGameRunning = false;

                var winner = _playersControl.CurPlayer.Side.ToOpposite();

                FinalGameState = new GameState(_modelController.Field, StateType.Finish, winner);

                _playersControl.BlackPlayer.StopTurn();
                _playersControl.WhitePlayer.StopTurn();
            }
        }

        public void Undo(int deep)
        {
            _modelController.Undo(deep);
            HistoryTraverser();
        }

        #endregion

        private void HistoryTraverser()
        {
            _isHistoryRolling = true;
            _playersControl.CurPlayer.Player.StopTurn();
            _playersControl.ChangeStateForNextGet(PlayerStateMachine.MachineState.Repeat);
        }

        private async Task<GameState> MakeTurnAsync()
        {
            if (!_isGameRunning)
            {
                if (FinalGameState == null) StopGame();
                return FinalGameState.Value;
            }

            var (Player, Side) = _playersControl.GetNextPlayer();

            // Get turn
            var newTurn = await Task.Run(() => Player.MakeTurn(_modelController.Field))
                .ConfigureAwait(continueOnCapturedContext: false);

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
                    _isGameRunning = false;
                    var winner = success ? Side : Side.ToOpposite();

                    FinalGameState = new GameState(_modelController.Field, StateType.Finish, winner);

                    return FinalGameState.Value;
                }
            }

            _isHistoryRolling = false;

            return new GameState(_modelController.Field, StateType.Turn, Side);
        }

        private bool TryUpdateField(IGameTurn newTurn)
        {
            if (newTurn == null ||
                !FieldUtils.TryCreateField(_modelController.Field, newTurn, out GameField newGameField))
            {
                return false;
            }

            _modelController.UpdateField(newGameField);

            return true;
        }
    }
}
