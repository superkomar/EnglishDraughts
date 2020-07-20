using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Utils;

namespace Core.Model
{
    public readonly struct GameField
    {
        internal GameField(IReadOnlyList<CellState> field, GameFieldCache fieldCache, int dimension)
        {
            FieldCache = fieldCache;
            Field = field;
            Dimension = dimension;
        }

        public int Dimension { get; }
        
        public IReadOnlyList<CellState> Field { get; }
        
        internal GameFieldCache FieldCache { get; }

        public CellState this[int idx] => Field.ElementAtOrDefault(idx);
    }
}
