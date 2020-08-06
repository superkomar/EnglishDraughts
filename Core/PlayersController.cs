using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Model;
using Core.Utils;

namespace Core
{
    public interface IPlayerParameters
    {
        int CalculationTime { get; }

        PlayerSide Side { get; }
    }

    public sealed class GameHistory
    {
        private readonly Stack<GameField> _redoStack = new Stack<GameField>();
        private readonly Stack<GameField> _undoStack = new Stack<GameField>();

        public void Push(GameField gameField)
        {
            _undoStack.Push(gameField);
            _redoStack.Clear();
        }

        public GameField Redo() => TryMoveElement(_redoStack, _undoStack);

        public GameField Undo() => TryMoveElement(_undoStack, _redoStack);

        private static GameField TryMoveElement(Stack<GameField> src, Stack<GameField> dst)
        {
            if (src.TryPop(out GameField gameField))
            {
                dst.Push(gameField);
            }

            return gameField;
        }
    }

    public class PlayersController
    {
        private CancellationTokenSource _cancellationSource;

        public PlayersController(int dimension, IGamePlayer whitePlayer, IGamePlayer blackPlayer, IStatusReporter reporter)
        {
            _cancellationSource = new CancellationTokenSource();

            Dimension = dimension;
            GameField = ModelsCreator.CreateGameField(Dimension);
            History = new GameHistory();

            BlackPlayer = blackPlayer;
            WhitePlayer = whitePlayer;

            WhitePlayer.StartGame(Dimension, PlayerSide.White, reporter);
            BlackPlayer.StartGame(Dimension, PlayerSide.Black, reporter);
        }

        public IGamePlayer BlackPlayer { get; }

        public IGamePlayer WhitePlayer { get; }

        public GameField GameField { get; private set; }

        public int Dimension { get; }

        public bool IsGameRunning { get; private set; }

        public async void StartGame()
        {
            IsGameRunning = true;

            while (IsGameRunning)
            {
                await MakeTurn(BlackPlayer, PlayerSide.Black);
                await MakeTurn(WhitePlayer, PlayerSide.White);
            }
        }

        public GameHistory History { get; private set; }

        private async Task MakeTurn(IGamePlayer player, PlayerSide side)
        {
            var taskTurn = player.MakeTurnAsync(GameField, side);//, _cancellationSource.Token);

            //var newTurn = await player.MakeTurn(GameField, side);
            var newTurn = await taskTurn;

            //var tmp = taskTurn.Status;

            if (!GameFieldUpdater.TryMakeTurn(GameField, newTurn, out GameField newGameField))
            {
                IsGameRunning = false;
                Winner = side.ToOpposite();
            }
            else if (GameRules.IsPlayerWin(newGameField, side))
            {

            }

            GameField = newGameField;

            History.Push(newGameField);
        }

        public PlayerSide Winner { get; private set; }

        public void StopGame()
        {

        }
    }
}
