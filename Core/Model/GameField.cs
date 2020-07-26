using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Core.Enums;
using Core.Helpers;

[assembly: InternalsVisibleTo("NUnitTests")]
namespace Core.Model
{
    public readonly struct GameField
    {
        internal GameField(IReadOnlyList<CellState> field, NeighborsHelper neighborsHelper, int dimension)
        {
            NeighborsHelper = neighborsHelper;
            Field = field;
            Dimension = dimension;
        }

        public int Dimension { get; }
        
        public IReadOnlyList<CellState> Field { get; }
        
        public NeighborsHelper NeighborsHelper { get; }

        public CellState this[int idx] => Field.ElementAtOrDefault(idx);
    }
}
