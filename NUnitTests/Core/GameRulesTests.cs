using System.Collections.Generic;

using Core.Enums;
using Core.Helpers;
using Core.Model;
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
            //  0(w)| 1(b)| 2(w)| 3(b)| 4(w)| 5(b)| 6(w)| 7(b)
            //  8(b)| 9(w)|10(b)|11(w)|12(b)|13(w)|14(b)|15(w)
            // 16(w)|17(b)|18(w)|19(b)|20(w)|21(b)|22(w)|23(b)
            // 24(b)|25(w)|26(b)|27(w)|28(b)|29(w)|30(b)|31(w)
            // 32(w)|33(b)|34(w)|35(b)|36(w)|37(b)|38(w)|39(b)
            // 40(b)|41(w)|42(b)|43(w)|44(b)|45(w)|46(b)|47(w)
            // 48(w)|49(b)|50(w)|51(b)|52(w)|53(b)|54(w)|55(b)
            // 56(b)|57(w)|58(b)|59(w)|60(b)|61(w)|62(b)|63(w)

            var field = new List<CellState>();
            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    field.Add(CellState.Empty);
                }
            }

            field[1] = field[17] = field[56] = CellState.BlackMen;
            field[3] = field[19] = field[58] = CellState.BlackKing;
            field[5] = field[21] = field[60] = CellState.WhiteMen;
            field[7] = field[23] = field[62] = CellState.WhiteKing;

            field[49] = CellState.WhiteKing;
            field[51] = CellState.WhiteMen;
            field[53] = CellState.BlackKing;
            field[55] = CellState.BlackMen;

            _gameField = new GameField(field, new NeighborsHelper(Dimension), Dimension);
        }

        [Test]
        public void Test_CanLevelUp()
        {
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 1));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 3));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField,  7));
            
            Assert.IsTrue(GameRules.CanLevelUp(_gameField,  5));

            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 17));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 19));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 21));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 23));

            Assert.IsTrue(GameRules.CanLevelUp(_gameField,  56));
            
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 58));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 60));
            Assert.IsFalse(GameRules.CanLevelUp(_gameField, 62));
        }

        [Test]
        public void Test_IsValidCellIdx()
        {
            Assert.IsTrue(GameRules.IsValidCellIdx(_gameField, 1));
            Assert.IsTrue(GameRules.IsValidCellIdx(_gameField, 3));

            Assert.IsFalse(GameRules.IsValidCellIdx(_gameField, -1));
            Assert.IsFalse(GameRules.IsValidCellIdx(_gameField, _gameField.Dimension * _gameField.Dimension));
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
