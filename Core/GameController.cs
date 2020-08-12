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
    public class GameFinishEventArgs : EventArgs
    {
        public PlayerSide Winner { get; set; }
    }

    public class FieldUpdateEventArgs : EventArgs
    {
        public GameField Field { get; set; }
    }

    public interface IGameController
    {
        event EventHandler<FieldUpdateEventArgs> FieldUpdated;

        event EventHandler<GameFinishEventArgs> GameEnded;

        int Dimension { get; }

        bool IsGameRunning { get; }

        GameField CurrentField { get; }

        void StartGame();

        void StopGame();
        
        bool Redo(out GameField field);

        bool Undo(out GameField field);
    }

    public class GameController
    {
        public enum PlayerType
        {
            Human,
            Robot
        }

        private const int DefaultRobotCalculationTime = 10000;

        private readonly IStatusReporter _reporter;
        private readonly ModelController _modelController;
        private readonly IGamePlayer _blackPlayer;
        private readonly IGamePlayer _whitePlayer;

        private CancellationTokenSource _cancellationSource;

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

        public int Dimension => _modelController.Dimension;

        public GameField GameField => _modelController.Field;

        public bool IsGameRunning { get; private set; }

        public PlayerSide Winner { get; private set; }

        private static async Task<T> TimerTask<T>(int timerMs)
        {
            await Task.Delay(timerMs);
            return default;
        }

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

        public event EventHandler<FieldUpdateEventArgs> FieldUpdated;

        private void OnFieldUpdateChanged(GameField newField) =>
            FieldUpdated?.Invoke(this, new FieldUpdateEventArgs { Field = newField });

        private IGamePlayer _currentPlayer;

        private Func<IGamePlayer, PlayerSide, Task<IGameTurn>> GetLaunchStartegy(PlayerType type) =>
            type switch
            {
                PlayerType.Robot => MakeRobotTurn,
                PlayerType.Human => MakeHumanTurn,
                _ => throw new NotImplementedException()
            };

        private async Task<IGameTurn> MakeHumanTurn(IGamePlayer player, PlayerSide side)
        {
            IGameTurn newTurn = null;

            bool repeatTurn;

            void HistoryHandler(object sender, EventArgs e) => _cancellationSource?.Cancel();

            do
            {
                repeatTurn = false;
                _cancellationSource = CancellationTokenSource.CreateLinkedTokenSource();

                try
                {
                    _modelController.HistoryRolling += HistoryHandler;
                    newTurn = await Task.Run(() => player.MakeTurnAsync(GameField, side), _cancellationSource.Token);
                }
                catch (TaskCanceledException)
                {
                    repeatTurn = true;
                }
                catch (Exception)
                {
                    repeatTurn = false;
                }
                finally
                {
                    _modelController.HistoryRolling -= HistoryHandler;
                }

            } while (repeatTurn);

            return newTurn;
        }

        private async Task<IGameTurn> MakeRobotTurn(IGamePlayer player, PlayerSide side)
        {
            var time = player.Parameters.TurnTime > 0
                ? player.Parameters.TurnTime
                : DefaultRobotCalculationTime;

            IGameTurn newTurn = await await Task.WhenAny(
                TimerTask<IGameTurn>(time),
                player.MakeTurnAsync(GameField, side));

            return newTurn;
        }

        private async Task MakeTurn(IGamePlayer player, PlayerSide side)
        {
            if (!IsGameRunning)
            {
                return;
            }

            IGameTurn newTurn = null;

            _reporter?.Report($"{side} player turn");

            var taskTurn = GetLaunchStartegy(player.Parameters.Type);

            try
            {
                newTurn = await taskTurn(player, side);
            }
            catch (Exception ex)
            {

            }

            if (TryUpdateField(newTurn, side))
            { }

            if (newTurn == null
                || !GameFieldUpdater.TryMakeTurn(GameField, newTurn, out GameField newGameField)
                || GameRules.IsPlayerWin(newGameField, side))
            {
                IsGameRunning = false;
                Winner = side.ToOpposite();

                _reporter.Report($"{Winner} player win.");
            }
            else
            {
                _modelController.UpdateField(newGameField);
            }
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

            if (GameRules.IsPlayerWin(newGameField, side))
            {
                IsGameRunning = false;
                Winner = side.ToOpposite();

                _reporter?.Report($"{Winner} player win.");
            }
        }
    }
}
