using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Models;
using Core.Utils;

namespace NUnitTests.CoreTests
{
    public static class CommonValues
    {
        static CommonValues()
        {
            var dimension = 8;

            DefaultField = GameFieldUtils.CreateField(dimension);

            ShortJumpsField = new GameField(
                GenerateFieldWithJumps(dimension, false),
                new NeighborsFinder(dimension),
                dimension);

            LongJumpsField = new GameField(
                GenerateFieldWithJumps(dimension, true),
                new NeighborsFinder(dimension),
                dimension);
        }

        public static GameField DefaultField { get; }
        
        public static GameField LongJumpsField { get; }

        public static GameField ShortJumpsField { get; }
        
        private static List<CellState> FillLoingJumps(List<CellState> cells)
        {
            //  0|  (-)|(Wk)| (-)| (+)| (-)| (+)| (-)|(Bm) | 7
            //  8|  (+)| (-)|(Bm)| (-)|(Wk)| (-)|(Wm)| (-) |15
            // 16|  (-)| (+)| (-)| (+)| (-)| (+)| (-)| (+) |23
            // 24|  (+)| (-)| (+)| (-)|(Bm)| (-)|(Wm)| (-) |31
            // 32|  (-)| (+)| (-)| (+)| (-)| (+)| (-)| (+) |39
            // 40|  (+)| (-)|(Bm)| (-)|(Bm)| (-)|(Wm)| (-) |47
            // 48|  (-)| (+)| (-)| (+)| (-)| (+)| (-)|(Bk) |55
            // 56| (Wk)| (-)| (+)| (-)| (+)| (-)| (+)| (-) |63

            cells[ 7] = cells[42] = cells[44] = CellState.BlackMan;
            cells[10] = cells[28] = cells[55] = CellState.BlackKing;
            cells[14] = cells[30] = cells[46] = CellState.WhiteMan;
            cells[ 1] = cells[12] = cells[56] = CellState.WhiteKing;

            return cells;
        }

        private static List<CellState> FillShortJumps(List<CellState> cells)
        {
            //  0|  (-)|(Bm)| (-)|(Bk)| (-)|(Wm)| (-)|(Wk) | 7
            //  8|  (+)| (-)| (+)| (-)| (+)| (-)| (+)| (-) |15
            // 16|  (-)|(Bm)| (-)|(Bk)| (-)|(Wm)| (-)|(Wk) |23
            // 24| (Wk)| (-)|(Wm)| (-)|(Bk)| (-)|(Bm)| (-) |31
            // 32|  (-)| (+)| (-)| (+)| (-)| (+)| (-)| (+) |39
            // 40|  (+)| (-)| (+)| (-)| (+)| (-)| (+)| (-) |47
            // 48|  (-)|(Bm)| (-)|(Bk)| (-)|(Wm)| (-)|(Wk) |55
            // 56| (Bm)| (-)|(Bk)| (-)|(Wm)| (-)|(Wk)| (-) |63

            cells[1] = cells[17] = cells[30] = cells[49] = cells[56] = CellState.BlackMan;
            cells[3] = cells[19] = cells[28] = cells[51] = cells[58] = CellState.BlackKing;
            cells[5] = cells[21] = cells[26] = cells[53] = cells[60] = CellState.WhiteMan;
            cells[7] = cells[23] = cells[24] = cells[55] = cells[62] = CellState.WhiteKing;

            cells[26] = CellState.WhiteMan;

            return cells;
        }

        private static List<CellState> GenerateEmptyField(int dimension) =>
            Enumerable.Repeat(CellState.Empty, dimension * dimension).ToList();

        private static List<CellState> GenerateFieldWithJumps(int dimension, bool isJumpLong) =>
            isJumpLong ? FillLoingJumps(GenerateEmptyField(dimension)) : FillShortJumps(GenerateEmptyField(dimension));
    }
}
