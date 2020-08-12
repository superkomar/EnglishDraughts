namespace Core.Interfaces
{
    public interface IGameHistory<T>
    {
        T Current { get; }

        void Add(T newItem);

        T Undo();

        T Redo();
    }
}
