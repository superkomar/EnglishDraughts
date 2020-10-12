using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;

namespace Core.Models
{
    class FieldCellCollection : IReadOnlyList<CellState>
    {
        private const int CellStateBitMask = 7;
        private const int CellStateBitSize = 3;
        private const int CellStateIntCount = 10;

        private readonly int[] _field;

        public FieldCellCollection(IReadOnlyList<CellState> field, int dimension)
        {
            Dimension = dimension;
            Count = Dimension * Dimension;

            var result = new int[(Count + 2 * (CellStateIntCount - 1)) / CellStateIntCount / 2];

            foreach (var (state, idx) in field.Select((x, idx) => (x, idx)))
            {
                var (rowIdx, colIdx) = GePositionIdx(idx, Dimension);

                if ((rowIdx + colIdx) % 2 == 0) continue;

                var (intIdx, innerIdx) = GePositionIdx(idx / 2, CellStateIntCount);

                result[intIdx] = ToInt(result[intIdx], (int)state, innerIdx);
            }

            _field = result;
        }

        public int Count { get; }
        
        public int Dimension { get; }
        
        public CellState this[int index]
        {
            get
            {
                var (rowIdx, colIdx) = GePositionIdx(index, Dimension);

                if ((rowIdx + colIdx) % 2 == 0) return CellState.Empty;

                var (curIdx, innerIdx) = GePositionIdx(index / 2, CellStateIntCount);

                return (CellState)FromInt(_field[curIdx], innerIdx);
            }
        }

        public IEnumerator<CellState> GetEnumerator() =>
            Enumerable.Range(0, Count).Select(x => this[x]).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private static int FromInt(int src, int idx)
        {
            if (idx > CellStateIntCount) return 0;

            int mask = CellStateBitMask << (idx * CellStateBitSize);
            return (src & mask) >> (idx * CellStateBitSize);
        }

        private static (int, int) GePositionIdx(int idx, int size) => (idx / size, idx % size);

        private static int ToInt(int dst, int value, int idx)
        {
            if (idx > CellStateIntCount) return dst;

            var shiftedValue = value << (idx * CellStateBitSize);
            return dst | shiftedValue;
        }
    }
}
