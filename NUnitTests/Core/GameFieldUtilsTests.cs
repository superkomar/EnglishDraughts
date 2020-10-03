using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Models;
using Core.Utils;

using NUnit.Framework;

namespace NUnitTests.Core
{
    public class GameFieldUtilsTests
    {
        private const int Dimension = 8;

        private GameField _gameField;

        [OneTimeSetUp]
        public void GenerateField()
        {
            //  0(-)| 1(+)| 2(-)| 3(+)| 4(-)| 5(+)| 6(-)| 7(+)
            //  8(+)| 9(-)|10(+)|11(-)|12(+)|13(-)|14(+)|15(-)
            // 16(-)|17(+)|18(-)|19(+)|20(-)|21(+)|22(-)|23(+)
            // 24(+)|25(-)|26(+)|27(-)|28(+)|29(-)|30(+)|31(-)
            // 32(-)|33(+)|34(-)|35(+)|36(-)|37(+)|38(-)|39(+)
            // 40(+)|41(-)|42(+)|43(-)|44(+)|45(-)|46(+)|47(-)
            // 48(-)|49(+)|50(-)|51(+)|52(-)|53(+)|54(-)|55(+)
            // 56(+)|57(-)|58(+)|59(-)|60(+)|61(-)|62(+)|63(-)

            var field = new List<CellState>();
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    field.Add(CellState.Empty);
                }
            }

            field[1] = field[17] = field[49] = field[56] = CellState.BlackMen;
            field[3] = field[19] = field[51] = field[58] = CellState.BlackKing;
            field[5] = field[21] = field[53] = field[60] = CellState.WhiteMen;
            field[7] = field[23] = field[55] = field[62] = CellState.WhiteKing;

            field[24] = CellState.WhiteKing;
            field[26] = CellState.WhiteMen;
            field[28] = CellState.BlackKing;
            field[30] = CellState.BlackMen;

            _gameField = new GameField(field, new NeighborsHelper(Dimension), Dimension);
        }

        [Test]
        public void GameFieldUtils_FindRequiredJumps()
        {
            var jumpsBlack = GameFieldUtils.FindRequiredJumps(_gameField, PlayerSide.Black).ToArray();
            var jumpsWhite = GameFieldUtils.FindRequiredJumps(_gameField, PlayerSide.White).ToArray();

            Assert.AreEqual(jumpsBlack.Length, 3);
            Assert.AreEqual(jumpsWhite.Length, 5);

            Assert.AreEqual(jumpsBlack[0].Turns, new int[] { 17, 26, 35 });
            Assert.AreEqual(jumpsBlack[1].Turns, new int[] { 19, 26, 33 });
            Assert.AreEqual(jumpsBlack[2].Turns, new int[] { 28, 21, 14 });

            Assert.AreEqual(jumpsWhite[0].Turns, new int[] { 23, 30, 37 });
            Assert.AreEqual(jumpsWhite[1].Turns, new int[] { 24, 17, 10 });
            Assert.AreEqual(jumpsWhite[2].Turns, new int[] { 26, 17,  8 });
            Assert.AreEqual(jumpsWhite[3].Turns, new int[] { 26, 19, 12 });
            Assert.AreEqual(jumpsWhite[4].Turns, new int[] { 60, 51, 42 });
        }
    }
}
