using System.Collections.Generic;

using Core.Interfaces;

namespace Core.Model
{
    public sealed class GameHistory<T> : IGameHistory<T>
    {
        private readonly List<T> _history;

        private int _curIndex;
        private int _maxIndex;

        public GameHistory(T initItem)
        {
            _maxIndex = -1;
            _curIndex = -1;
            _history = new List<T>();

            Add(initItem);
        }

        public T Current => _history[_curIndex];

        public void Add(T item)
        {
            if (item == null)
            {
                return;
            }

            _curIndex++;
            _maxIndex = _curIndex;
            _history.Insert(_curIndex, item);
        }

        public T Redo()
        {
            _curIndex = _curIndex == _maxIndex ? _curIndex : _curIndex + 1;

            return _history[_curIndex];
        }

        public T Undo()
        {
            _curIndex = _curIndex == 0 ? _curIndex : _curIndex - 1;

            return _history[_curIndex];
        }
    }
}
