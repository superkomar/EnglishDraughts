using System.Collections.Generic;

using Core.Enums;
using Core.Models;
using Core.Utils;

using NUnit.Framework;

namespace NUnitTests.Core
{
    public class GameRulesTests
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

            field[1] = field[17] = field[56] = CellState.BlackMan;
            field[3] = field[19] = field[58] = CellState.BlackKing;
            field[5] = field[21] = field[60] = CellState.WhiteMan;
            field[7] = field[23] = field[62] = CellState.WhiteKing;

            field[49] = CellState.WhiteKing;
            field[51] = CellState.WhiteMan;
            field[53] = CellState.BlackKing;
            field[55] = CellState.BlackMan;

            _gameField = new GameField(field, new NeighborsFinder(Dimension), Dimension);
        }

        [Test]
        public void Test_CanLevelUp()
        {
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[1], 1));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[3], 3));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[7], 7));
            
            Assert.IsTrue(GameRules.CanLevelUp(_gameField, _gameField[5],  5));

            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[17], 17));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[19], 19));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[21], 21));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[23], 23));

            Assert.IsTrue(GameRules.CanLevelUp(_gameField, _gameField[56],  56));
            
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[58], 58));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[60], 60));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, _gameField[62], 62));
        }

        [Test]
        public void Test_IsValidCellIdx()
        {
            Assert.IsTrue(_gameField.IsValidCellIdx(1));
            Assert.IsTrue(_gameField.IsValidCellIdx(3));

            Assert.IsFalse(_gameField.IsValidCellIdx(-1));
            Assert.IsFalse(_gameField.IsValidCellIdx(_gameField.Dimension * _gameField.Dimension));
        }

        [Test]
        public void Test_IsValidTurnEnd()
        {
            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, -1));

            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, 1));
            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, 3));
            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, 5));
            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, 7));

            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, 17));
            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, 19));
            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, 21));
            Assert.IsFalse(GameRules.IsValidTurnEnd(_gameField, 23));

            Assert.IsTrue(GameRules.IsValidTurnEnd(_gameField, 8));
            Assert.IsTrue(GameRules.IsValidTurnEnd(_gameField, 10));
            Assert.IsTrue(GameRules.IsValidTurnEnd(_gameField, 12));
            Assert.IsTrue(GameRules.IsValidTurnEnd(_gameField, 14));
        }

        [Test]
        public void Test_IsValidTurnStart()
        {
            Assert.IsFalse(GameRules.IsValidTurnStart(_gameField, -1));

            Assert.IsTrue(GameRules.IsValidTurnStart(_gameField, 1));
            Assert.IsTrue(GameRules.IsValidTurnStart(_gameField, 3));
            Assert.IsTrue(GameRules.IsValidTurnStart(_gameField, 5));
            Assert.IsTrue(GameRules.IsValidTurnStart(_gameField, 7));

            Assert.IsTrue(GameRules.IsValidTurnStart(_gameField, 17));
            Assert.IsTrue(GameRules.IsValidTurnStart(_gameField, 19));
            Assert.IsTrue(GameRules.IsValidTurnStart(_gameField, 21));
            Assert.IsTrue(GameRules.IsValidTurnStart(_gameField, 23));

            Assert.IsFalse(GameRules.IsValidTurnStart(_gameField, 8));
            Assert.IsFalse(GameRules.IsValidTurnStart(_gameField, 10));
            Assert.IsFalse(GameRules.IsValidTurnStart(_gameField, 12));
            Assert.IsFalse(GameRules.IsValidTurnStart(_gameField, 14));
        }

        [Test]
        public void Test_IsMovePossible()
        {
            // Incorrect
            Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 1, -1));
            Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, -1, 1));
            Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, -1, -1));
            Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, 1, -1));
            Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, -1, 1));
            Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, -1, -1));

            // Second row
            {
                // BlackMen
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 17, 8));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 17, 10));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 17, 24));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 17, 26));

                // BlackKing
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 19, 10));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 19, 12));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 19, 26));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 19, 28));

                // WhiteMen
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.White, 21, 12));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.White, 21, 14));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, 21, 28));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, 21, 30));

                // WhiteKing
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.White, 23, 14));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.White, 23, 30));
            }

            // Sixth row
            {
                // WhiteKing
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.White, 49, 40));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.White, 49, 42));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, 49, 56));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, 49, 58));

                // WhiteMen
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.White, 51, 42));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.White, 51, 44));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, 51, 58));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.White, 51, 60));

                // BlackKing
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 53, 44));
                Assert.IsTrue(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 53, 46));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 53, 60));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 53, 62));

                // WhiteMen
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 55, 46));
                Assert.IsFalse(GameRules.IsMovePossible(_gameField, PlayerSide.Black, 55, 62));
            }
        }
    }
}
