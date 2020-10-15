using System;

namespace Core.Models
{
    public class NeighborsFinder
    {
        private readonly int _cellCount;
        private readonly int _dimension;

        internal enum DirectionType
        {
            LeftTop,
            LeftBot,
            RightTop,
            RightBot
        }

        public NeighborsFinder(int dimension)
        {
            if (dimension <= 0) throw new ArgumentException(@"Dimension should be more than zero");
            //if (dimension % 2 != 0) throw new ArgumentException(@"Dimention must be multiple of two");

            _dimension = dimension;
            _cellCount = _dimension * _dimension;
        }

        public CellNeighbors this[int idx] => this[idx / _dimension, idx % _dimension];

        public CellNeighbors this[int posX, int posY] => new CellNeighbors(
            leftTop:  CheckCellIdx(posX, _dimension * (posX - 1) + posY - 1),
            leftBot:  CheckCellIdx(posX, _dimension * (posX + 1) + posY - 1),
            rightTop: CheckCellIdx(posX, _dimension * (posX - 1) + posY + 1),
            rightBot: CheckCellIdx(posX, _dimension * (posX + 1) + posY + 1));

        public int GetLeftBotCell(int cellIdx, int deep = 1) => GetCellByDirection(cellIdx, deep, DirectionType.LeftBot);

        public int GetLeftTopCell(int cellIdx, int deep = 1) => GetCellByDirection(cellIdx, deep, DirectionType.LeftTop);

        public int GetRightBotCell(int cellIdx, int deep = 1) => GetCellByDirection(cellIdx, deep, DirectionType.RightBot);

        public int GetRightTopCell(int cellIdx, int deep = 1) => GetCellByDirection(cellIdx, deep, DirectionType.RightTop);

        public bool AreNeighbors(int firstId, int secondId) => this[firstId].IsNeighbor(secondId);

        internal int GetCellByDirection(int startIdx, int deep, DirectionType direction)
        {
            var endIdx = startIdx;

            if (deep < 1) return -1;

            for (var i = deep; i > 0 && endIdx != -1; i--)
            {
                switch (direction)
                {
                    case DirectionType.LeftBot: endIdx = this[endIdx].LeftBot; break;
                    case DirectionType.LeftTop: endIdx = this[endIdx].LeftTop; break;
                    case DirectionType.RightBot: endIdx = this[endIdx].RightBot; break;
                    case DirectionType.RightTop: endIdx = this[endIdx].RightTop; break;
                }
            }

            return endIdx;
        }

        private int CheckCellIdx(int curRow, int idxForCheck) =>
            idxForCheck >= 0 && idxForCheck < _cellCount && Math.Abs(curRow - (idxForCheck / _dimension)) == 1
            ? idxForCheck
            : -1;
    }
}
