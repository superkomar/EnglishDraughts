using Core.Model;

namespace Core.Interfaces
{
    public interface IGameHistory
    {
        public void Push(GameField gameField);

        public GameField Undo();

        public GameField Redo();
    }
}
