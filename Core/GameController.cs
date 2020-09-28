using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public GameController(int dimension, PlayerBase blackPlayer, PlayerBase whitePlayer)
        {
            if (whitePlayer == null || blackPlayer == null) throw new ArgumentNullException();

            _modelController = new ModelController(dimension);

            _playersControl = new PlayerStateMachine(blackPlayer, whitePlayer);

            _isGameRunning = _isHistoryRolling = false;
        }

        #region IGameController

        public void Undo(int deep)
        {
            _modelController.Undo(deep);
            HistoryRolling();
        }

        public void Redo(int deep)
        {
            _modelController.Redo(deep);
            HistoryRolling();
        }       

        public GameState? FinalGameState { get; private set; }

        public async IAsyncEnumerable<GameState> StartGameAsync()
        {
            if (FinalGameState != null)
            {
                yield return FinalGameState.Value;
                yield break;
            }

            yield return new GameState(_modelController.Field, GameState.StateType.Start, _playersControl.CurPlayer.Side);

            _isGameRunning = true;

            while (_isGameRunning)
            {
                yield return await MakeTurnAsync();
            }
        }

        public void StopGame()
        {
            _isGameRunning = false;

            var winner = _playersControl.CurPlayer.Side.ToOpposite();

            FinalGameState = new GameState(_modelController.Field, GameState.StateType.Finish, winner);

            _playersControl.BlackPlayer.StopMakeTurn();
            _playersControl.WhitePlayer.StopMakeTurn();
        }

        #endregion

        private void HistoryRolling()
        {
            _isHistoryRolling = true;
            _playersControl.CurPlayer.Player.StopMakeTurn();
            _playersControl.ChangeStateForOneGet(PlayerStateMachine.MachineState.Repeat);
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
            var newTurn = await Task.Run(() => Player.MakeTurn(_modelController.Field));

            // Check if game stoped
            if (FinalGameState != null) return FinalGameState.Value;

            // Check if returned value the reason of history rolling
            if (!_isHistoryRolling)
            {
                // Check turn
                var success = TryUpdateField(newTurn);

                // Check end of game
                if (!success || GameRules.IsPlayerWin(_modelController.Field, Side))
                {
                    _isGameRunning = false;
                    var winner = success ? Side : Side.ToOpposite();

                    FinalGameState = new GameState(_modelController.Field, GameState.StateType.Finish, Side);

                    return FinalGameState.Value;
                }
            }

            _isHistoryRolling = false;

            return new GameState(_modelController.Field, GameState.StateType.Turn, Side);
        }

        private bool TryUpdateField(IGameTurn newTurn)
        {
            if (newTurn == null ||
                !GameFieldUtils.TryMakeTurn(_modelController.Field, newTurn, out GameField newGameField))
            {
                return false;
            }

            _modelController.UpdateField(newGameField);

            return true;
        }
    }
}
