using System;
using System.Threading.Tasks;

using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Model;
using Core.Utils;

namespace Core
{
    public interface IGameController
    {
        event EventHandler<FieldUpdateEventArgs> FieldUpdated;

        event EventHandler<GameFinishEventArgs> GameEnded;

        GameField CurrentField { get; }
        
        int Dimension { get; }

        bool IsGameRunning { get; }
        
        bool Redo(out GameField field);

        void StartGame();

        void StopGame();

        bool Undo(out GameField field);
    }

    public class FieldUpdateEventArgs : EventArgs
    {
        public GameField Field { get; set; }
    }

    public class GameFinishEventArgs : EventArgs
    {
        public PlayerSide Winner { get; set; }
    }

    public class GameController
    {
        private readonly IPlayerLauncher _blackLauncher;
        private readonly ModelController _modelController;
        private readonly IStatusReporter _reporter;
        private readonly IPlayerLauncher _whiteLauncher;

        private IPlayerLauncher _curLauncher;

        public GameController(int dimension, IPlayerLauncher whitePlayerLauncher, IPlayerLauncher blackPlayerLauncher, IStatusReporter reporter)
        {
            _modelController = new ModelController(dimension);

            _reporter = reporter;

            _blackLauncher = blackPlayerLauncher;
            _whiteLauncher = whitePlayerLauncher;

            _whiteLauncher.InitGame(Dimension, PlayerSide.White, reporter);
            _blackLauncher.InitGame(Dimension, PlayerSide.Black, reporter);

            Winner = PlayerSide.None;
        }

        public event EventHandler<FieldUpdateEventArgs> FieldUpdated;

        public int Dimension => _modelController.Dimension;

        public GameField GameField => _modelController.Field;

        public bool IsGameRunning { get; private set; }

        public PlayerSide Winner { get; private set; }

        public async Task StartGame()
        {
            IsGameRunning = true;

            while (IsGameRunning)
            {
                await MakeTurn(_blackLauncher, PlayerSide.Black);
                await MakeTurn(_whiteLauncher, PlayerSide.White);
            }
        }

        public void StopGame()
        {
            if (IsGameRunning)
            {
                IsGameRunning = false;
                _curLauncher?.FinishGame();
            }
        }
        private async Task MakeTurn(IPlayerLauncher launcher, PlayerSide side)
        {
            if (!IsGameRunning)
            {
                return;
            }

            _reporter?.ReportInfo($"{side} player turn");

            _curLauncher = launcher;

            // Get turn
            var newTurn = await launcher.MakeTurnAsync(GameField, side);

            // Check turn
            var result = TryUpdateField(newTurn);

            // Check end of game
            if (!result || GameRules.IsPlayerWin(_modelController.Field, side))
            {
                IsGameRunning = false;
                Winner = side.ToOpposite();

                _reporter?.ReportInfo($"{Winner} player win.");
            }
        }

        private void OnFieldUpdateChanged(GameField newField) =>
            FieldUpdated?.Invoke(this, new FieldUpdateEventArgs { Field = newField });

        private bool TryUpdateField(IGameTurn newTurn)
        {
            if (newTurn == null ||
                !GameFieldUpdater.TryMakeTurn(GameField, newTurn, out GameField newGameField))
            {
                return false;
            }

            _modelController.UpdateField(newGameField);
            OnFieldUpdateChanged(_modelController.Field);

            return true;
        }
    }
}
