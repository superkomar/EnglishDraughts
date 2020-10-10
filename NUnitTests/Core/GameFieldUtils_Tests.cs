using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

using NUnit.Framework;

namespace NUnitTests.Core
{
    public class GameFieldUtils_Tests
    {
        private const int Dimension = 8;

        private GameField _customField;
        private GameField _defaultField;

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

            var cells = new List<CellState>();
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    cells.Add(CellState.Empty);
                }
            }

            cells[1] = cells[17] = cells[49] = cells[56] = CellState.BlackMen;
            cells[3] = cells[19] = cells[51] = cells[58] = CellState.BlackKing;
            cells[5] = cells[21] = cells[53] = cells[60] = CellState.WhiteMen;
            cells[7] = cells[23] = cells[55] = cells[62] = CellState.WhiteKing;

            cells[24] = CellState.WhiteKing;
            cells[26] = CellState.WhiteMen;
            cells[28] = CellState.BlackKing;
            cells[30] = CellState.BlackMen;

            _customField = new GameField(cells, new NeighborsHelper(Dimension), Dimension);
            
            _defaultField = ModelsCreator.CreateGameField(8);
        }

        [Test]
        public void GameFieldUtils_FindRequiredJumps()
        {
            var jumpsBlack = GameFieldUtils.FindRequiredJumps(_customField, PlayerSide.Black).ToArray();
            var jumpsWhite = GameFieldUtils.FindRequiredJumps(_customField, PlayerSide.White).ToArray();

            Assert.AreEqual(jumpsBlack.Length, 3);
            Assert.AreEqual(jumpsWhite.Length, 5);

            Assert.AreEqual(jumpsBlack[0].Steps, new int[] { 17, 26, 35 });
            Assert.AreEqual(jumpsBlack[1].Steps, new int[] { 19, 26, 33 });
            Assert.AreEqual(jumpsBlack[2].Steps, new int[] { 28, 21, 14 });

            Assert.AreEqual(jumpsWhite[0].Steps, new int[] { 23, 30, 37 });
            Assert.AreEqual(jumpsWhite[1].Steps, new int[] { 24, 17, 10 });
            Assert.AreEqual(jumpsWhite[2].Steps, new int[] { 26, 17,  8 });
            Assert.AreEqual(jumpsWhite[3].Steps, new int[] { 26, 19, 12 });
            Assert.AreEqual(jumpsWhite[4].Steps, new int[] { 60, 51, 42 });
        }

        [Test]
        public void FindTurnsForCell_Test_DefaultField()
        {
            var blackTurns = new List<IGameTurn>(GameFieldUtils.FindRequiredJumps(_defaultField, PlayerSide.Black).ToArray());
            var whiteTurns = new List<IGameTurn>(GameFieldUtils.FindRequiredJumps(_defaultField, PlayerSide.White).ToArray());

            Assert.AreEqual(blackTurns.Count, 0);
            Assert.AreEqual(whiteTurns.Count, 0);

            GetTuns(_defaultField, out blackTurns, out whiteTurns);

            // Black turns
            Assert.AreEqual(blackTurns.Count, 7);
            Assert.AreEqual(blackTurns[0].Steps, new int[] { 17, 24 });
            Assert.AreEqual(blackTurns[1].Steps, new int[] { 17, 26 });
            Assert.AreEqual(blackTurns[2].Steps, new int[] { 19, 26 });
            Assert.AreEqual(blackTurns[3].Steps, new int[] { 19, 28 });
            Assert.AreEqual(blackTurns[4].Steps, new int[] { 21, 28 });
            Assert.AreEqual(blackTurns[5].Steps, new int[] { 21, 30 });
            Assert.AreEqual(blackTurns[6].Steps, new int[] { 23, 30 });

            // White turns
            Assert.AreEqual(whiteTurns.Count, 7);
            Assert.AreEqual(whiteTurns[0].Steps, new int[] { 40, 33 });
            Assert.AreEqual(whiteTurns[1].Steps, new int[] { 42, 33 });
            Assert.AreEqual(whiteTurns[2].Steps, new int[] { 42, 35 });
            Assert.AreEqual(whiteTurns[3].Steps, new int[] { 44, 35 });
            Assert.AreEqual(whiteTurns[4].Steps, new int[] { 44, 37 });
            Assert.AreEqual(whiteTurns[5].Steps, new int[] { 46, 37 });
            Assert.AreEqual(whiteTurns[6].Steps, new int[] { 46, 39 });
        }

        [Test]
        public void FindTurnsForCell_Test_CustomField()
        {
            GetTuns(_customField,
                out List<IGameTurn> blackTurns,
                out List<IGameTurn> whiteTurns);

            // Black turns
            Assert.AreEqual(blackTurns.Count, 3);
            Assert.AreEqual(blackTurns[0].Steps, new int[] { 17, 26, 35 });
            Assert.AreEqual(blackTurns[1].Steps, new int[] { 19, 26, 33 });
            Assert.AreEqual(blackTurns[2].Steps, new int[] { 28, 21, 14 });

            // White turns
            Assert.AreEqual(whiteTurns.Count, 5);
            Assert.AreEqual(whiteTurns[0].Steps, new int[] { 23, 30, 37 });
            Assert.AreEqual(whiteTurns[1].Steps, new int[] { 24, 17, 10 });
            Assert.AreEqual(whiteTurns[2].Steps, new int[] { 26, 17,  8 });
            Assert.AreEqual(whiteTurns[3].Steps, new int[] { 26, 19, 12 });
            Assert.AreEqual(whiteTurns[4].Steps, new int[] { 60, 51, 42 });
        }


        private void GetTuns(GameField field, out List<IGameTurn> blackTurns, out List<IGameTurn> whiteTurns)
        {
            var blackSimple = new List<IGameTurn>();
            var blackJumps  = new List<IGameTurn>();

            var whiteSimple = new List<IGameTurn>();
            var whiteJumps  = new List<IGameTurn>();

            static void Processor(IEnumerable<IGameTurn> turns, IList<IGameTurn> jumps, IList<IGameTurn> simples)
            {
                foreach (var turn in turns)
                {
                    if (turn.IsSimple) simples.Add(turn);
                    else jumps.Add(turn);
                }
            }

            for (var i = 0; i < field.Dimension; i++)
            {
                for (var j = 0; j < field.Dimension; j++)
                {
                    var cellIdx = i * field.Dimension + j;

                    if (field[cellIdx] == CellState.Empty) continue;

                    var cellTurns = GameFieldUtils.FindTurnsForCell(field, cellIdx);

                    if (field[cellIdx].IsSameSide(PlayerSide.Black))
                    {
                        Processor(cellTurns, blackJumps, blackSimple);
                    }
                    else
                    {
                        Processor(cellTurns, whiteJumps, whiteSimple);
                    }
                }
            }

            blackTurns = blackJumps.Any() ? blackJumps : blackSimple;
            whiteTurns = whiteJumps.Any() ? whiteJumps : whiteSimple;
        }
    }
}
