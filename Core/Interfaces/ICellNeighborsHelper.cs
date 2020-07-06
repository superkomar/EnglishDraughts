using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface ICellNeighborsHelper
    {
        IReadOnlyCollection<int> this[int cellId] { get; }

        IReadOnlyCollection<int> GetNeighbors(int cellId);

        bool IsNeighbors(int firstId, int secondId);
    }
}
