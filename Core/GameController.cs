using System;

using Core.Interfaces;
using Core.Model;

namespace Core
{
    public class GameController : IGameController
    {
        public void InitGameField(int dimension)
        {
            GameField = new GameField(dimension);
            NeighborsHelper = new CellNeighborsHelper(dimension);
        }

        public ICellNeighborsHelper NeighborsHelper { get; private set; } 

        public IGameField GameField { get; private set; }

        public IRulesChecker RulesChecker => throw new NotImplementedException();

        public bool TryChangeGameField(IGameStep step)
        {
            throw new NotImplementedException();
        }
    }
}
