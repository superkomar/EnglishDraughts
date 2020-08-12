using System;

using Core.Utils;

namespace Core.Model
{
    internal class ModelController
    {
        private readonly GameHistory<GameField> _history;

        public ModelController(int dimension)
        {
            Dimension = dimension;
            _history = new GameHistory<GameField>(ModelsCreator.CreateGameField(Dimension));
        }

        public event EventHandler HistoryRolling;

        public int Dimension { get; }

        public GameField Field => _history.Current;
        
        public GameField Redo()
        {
            var currField = Field;
            var newField = _history.Redo();

            if (currField.Equals(newField))
            {
                return Field;
            }

            OnHistoryRollingChanged();

            return Field;
        }

        public GameField Undo()
        {
            var currField = Field;
            var newField = _history.Undo();

            if (currField.Equals(newField))
            {
                return Field;
            }

            OnHistoryRollingChanged();

            return Field;
        }

        public void UpdateField(GameField newField)
        {
            _history.Add(newField);
        }
        
        private void OnHistoryRollingChanged() => HistoryRolling?.Invoke(null, EventArgs.Empty);
    }
}
