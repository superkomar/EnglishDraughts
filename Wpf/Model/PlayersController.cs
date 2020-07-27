using System.Collections.Generic;

using Core;
using Core.Enums;
using Core.Model;

namespace Wpf.Model
{
    public interface IPlayerParameters
    {
        int CalculationTime { get; }

        PlayerSide Side { get; }
    }

    public interface IPlayer
    {
        PlayerSide Side { get; }

        GameTurn GetTurn(GameField gameField, IPlayerParameters launchParameters = null);
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

    internal class PlayersController
    {
        public PlayersController()
        {
            History = new GameHistory();

            CurrentGameField = ModelsCreator.CreateGameField(Constants.FieldDimension);
        }

        public void StartGame(IPlayer blackPlayer, IPlayer whitePlayer)
        {
            while (IsGameRunning)
            {
                

            }
        }

        public GameHistory History { get; private set; }

        private void MakeTurn(IPlayer player)
        {
            var newTurn = player.GetTurn(CurrentGameField);

            if (!ModelsCreator.TryMakeTurn(CurrentGameField, newTurn, out GameField newGameField))
            {
                IsGameRunning = false;
                Winner = player.Side == PlayerSide.White ? PlayerSide.Black : PlayerSide.White;
            }

            History.Push(newGameField);
        }

        public GameField CurrentGameField { get; private set; }

        public bool IsGameRunning { get; private set; }

        public PlayerSide Winner { get; private set; }

        public void StopGame()
        {

        }
    }
}
