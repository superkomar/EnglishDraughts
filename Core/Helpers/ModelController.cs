using Core.Models;
using Core.Utils;

namespace Core.Helpers
{
    internal class ModelController
    {
        private readonly GameHistory<GameField> _history;

        public ModelController(int dimension)
        {
            _history = new GameHistory<GameField>(FieldUtils.CreateField(dimension));
        }

        public GameField Field => _history.Current;
        
        public GameField Redo(int deep = 1)
        {
            for (var i = 0; i < deep; i++)
            {
                var currField = Field;
                var newField = _history.Redo();

                if (currField.Equals(newField))
                {
                    return Field;
                }
            }

            return Field;
        }

        public GameField Undo(int deep = 1)
        {
            for (var i = 0; i < deep; i++)
            {
                var currField = Field;
                var newField = _history.Undo();

                if (currField.Equals(newField))
                {
                    return Field;
                }
            }

            return Field;
        }

        public void UpdateField(GameField newField) => _history.Add(newField);
    }
}
