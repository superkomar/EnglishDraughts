using System.Collections.Generic;

using Core.Interfaces;

namespace Core.Model
{
    internal sealed class GameHistory : IGameHistory
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
}
