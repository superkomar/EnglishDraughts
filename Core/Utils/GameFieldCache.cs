using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("NUnitTests")]
namespace Core.Utils
{
    internal readonly struct CellNeighbors
    {
        public CellNeighbors(int leftTop, int leftBot, int rightTop, int rightBot)
        {
            LeftTop = leftTop;
            LeftBot = leftBot;
            RightTop = rightTop;
            RightBot = rightBot ;
        }

        public int LeftBot { get; }
        
        public int LeftTop { get; }

        public int RightBot { get; }
        
        public int RightTop { get; }
        
        public bool IsNeighbor(int cellId) =>
            LeftBot == cellId || LeftTop == cellId || RightBot == cellId || RightTop == cellId;
    }

    internal class GameFieldCache
    {
        //private readonly int _width;
        //private readonly int _height;
        private readonly int _dimension;
        private readonly int _neighborsCount;
        private readonly IList<CellNeighbors> _neighbors;
        
        public GameFieldCache(int dimension)
        {
            _dimension = dimension;
            _neighborsCount = _dimension * _dimension;
            //_height = dimension;
            //_width = dimension; // todo: for dimension not multiple of two

            _neighbors = new List<CellNeighbors>(_dimension * _dimension);

            for (var i = 0; i < dimension; i++)
            {
                var startRowShift = i % 2;

                for (var j = 0; j < dimension; j++)
                {
                    if ((startRowShift + j) % 2 == 0)
                    {
                        _neighbors.Add(new CellNeighbors(-1, -1, -1, -1));
                    }
                    else
                    {
                        _neighbors[i] = new CellNeighbors(
                            leftTop:  CheckCellIdx1(i, dimension * (i - 1) + j - 1),
                            leftBot:  CheckCellIdx1(i, dimension * (i + 1) + j - 1),
                            rightTop: CheckCellIdx1(i, dimension * (i - 1) + j + 1),
                            rightBot: CheckCellIdx1(i, dimension * (i + 1) + j + 1)
                        );
                    }

                }
            }

            //for (var i = 0; i < _neighbors.Length; i++)
            //{
            //    var currRow = i / _width;
            //    var offset = i - ((currRow + 1) % 2);
            //    _neighbors[i] = new CellNeighbors(
            //        leftTop:  CheckCellIdx(currRow, offset - _width),
            //        leftBot:  CheckCellIdx(currRow, offset + _width),
            //        rightTop: CheckCellIdx(currRow, offset - _width + 1),
            //        rightBot: CheckCellIdx(currRow, offset + _width + 1)
            //    );
            //}
        }

        public CellNeighbors this[int idx] => _neighbors.ElementAtOrDefault(idx);

        public bool GetLeftBotCell(int cellIdx, out int leftBot, int deep = 1) =>
            GetCellByDirection(cellIdx, deep, DirectionType.LeftBot, out leftBot);

        public bool GetLeftTopCell(int cellIdx, out int leftTop, int deep = 1) =>
            GetCellByDirection(cellIdx, deep, DirectionType.LeftTop, out leftTop);

        public bool GetRightBotCell(int cellIdx, out int rightBot, int deep = 1) =>
            GetCellByDirection(cellIdx, deep, DirectionType.RightBot, out rightBot);

        public bool GetRightTopCell(int cellIdx, out int rightTop, int deep = 1) =>
            GetCellByDirection(cellIdx, deep, DirectionType.RightTop, out rightTop);

        public bool IsNeighbors(int firstId, int secondId) => this[firstId].IsNeighbor(secondId);

        //private int CheckCellIdx(int currRow, int resIdx) =>
        //    resIdx >= 0 && resIdx < _neighbors.Count && Math.Abs(currRow - resIdx / _width) == 1
        //    ? resIdx : -1;

        private int CheckCellIdx1(int curIdx, int idxForCheck) =>
            idxForCheck >= 0 && idxForCheck < _neighborsCount && Math.Abs((curIdx / _dimension) - (idxForCheck / _dimension)) == 1
            ? idxForCheck : -1;

        private bool GetCellByDirection(int startIdx, int deep, DirectionType direction, out int endIdx)
        {
            endIdx = startIdx;

            if (deep < 1) return false;

            for (var i = deep; i > 0 && endIdx != -1; i--)
            {
                switch (direction)
                {
                    case DirectionType.LeftBot:  endIdx = this[endIdx].LeftTop;  break;
                    case DirectionType.LeftTop:  endIdx = this[endIdx].LeftBot;  break;
                    case DirectionType.RightBot: endIdx = this[endIdx].RightBot; break;
                    case DirectionType.RightTop: endIdx = this[endIdx].RightTop; break;
                }
            }

            return endIdx != -1;
        }

        private enum DirectionType
        {
            LeftTop,
            LeftBot,
            RightTop,
            RightBot
        }
    }
}
