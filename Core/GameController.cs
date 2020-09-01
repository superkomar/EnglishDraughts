using System;
using System.Threading;
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

    public class GameController
    {
        private readonly IGamePlayer _blackPlayer;
        private readonly ModelController _modelController;
        private readonly IStatusReporter _reporter;
        private readonly IGamePlayer _whitePlayer;

        private CancellationTokenSource _cancellationSource;

        private IGamePlayer _currentPlayer;

        public GameController(int dimension, IGamePlayer whitePlayer, IGamePlayer blackPlayer, IStatusReporter reporter)
        {
            _modelController = new ModelController(dimension);

            _reporter = reporter;

            _blackPlayer = blackPlayer;
            _whitePlayer = whitePlayer;

            _whitePlayer.InitGame(Dimension, PlayerSide.White, reporter);
            _blackPlayer.InitGame(Dimension, PlayerSide.Black, reporter);

            Winner = PlayerSide.None;
        }

        public event EventHandler<FieldUpdateEventArgs> FieldUpdated;

        public int Dimension => _modelController.Dimension;

        public GameField GameField => _modelController.Field;

        public bool IsGameRunning { get; private set; }

        public PlayerSide Winner { get; private set; }

        public async void StartGame()
        {
            IsGameRunning = true;

            while (IsGameRunning)
            {
                await MakeTurn(_blackPlayer, PlayerSide.Black);
                await MakeTurn(_whitePlayer, PlayerSide.White);
            }
        }

        public void StopGame()
        {
            if (IsGameRunning)
            {
                IsGameRunning = false;
                _cancellationSource?.Cancel();
            }

        }
        private async Task MakeTurn(IGamePlayer player, PlayerSide side)
        {
            if (!IsGameRunning)
            {
                return;
            }

            _currentPlayer = player;

            _reporter?.Report($"{side} player turn");

            // Get turn
            var newTurn = await TryMakeTurn(player, side);

            // Check turn
            var result = TryUpdateField(newTurn, side);

            // Check end of game
            if (!result || GameRules.IsPlayerWin(_modelController.Field, side))
            {
                IsGameRunning = false;
                Winner = side.ToOpposite();

                _reporter?.Report($"{Winner} player win.");
            }
        }

        private void OnFieldUpdateChanged(GameField newField) => FieldUpdated?.Invoke(this, new FieldUpdateEventArgs { Field = newField });
        
        private async Task<IGameTurn> TryMakeTurn(IGamePlayer player, PlayerSide side)
        {
            IGameTurn newTurn = null;

            _cancellationSource?.Cancel();
            _cancellationSource = new CancellationTokenSource();

            try
            {
                newTurn = await Task.Run(() => player.MakeTurnAsync(GameField, side), _cancellationSource.Token);
            }
            catch (Exception ex)
            {
                newTurn = default;
            }

            return newTurn;
        }

        private bool TryUpdateField(IGameTurn newTurn, PlayerSide side)
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

    public class GameFinishEventArgs : EventArgs
    {
        public PlayerSide Winner { get; set; }
    }
}
