namespace Core.Interfaces
{
    public interface IGameController
    {
        ICellNeighborsHelper NeighborsHelper { get; }

        IGameField GameField { get; }

        IRulesChecker RulesChecker { get; }

        void InitGameField(int dimension);

        bool TryChangeGameField(IGameStep step);

    }
}
