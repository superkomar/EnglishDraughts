using System;
using System.Collections.Generic;

using Core.Enums;
using Core.Models;
using Core.Utils;

using NUnit.Framework;

using NUnitTests.Utils;

namespace NUnitTests.CoreTests
{
    [TestFixture]
    public class GameFieldUtilsTests
    {
        [Test]
        public void CreateField()
        {
            Assert.Throws<ArgumentException>(() => new GameField(-1));
            Assert.Throws<ArgumentException>(() => new GameField(0));

            var field_1 = new GameField(1);
            Assert.AreEqual(field_1.CellCount, 1);
            Assert.AreEqual(field_1[0], CellState.Empty);

            var field_2 = new GameField(2);
            Assert.AreEqual(field_2.CellCount, 4);
            Assert.AreEqual(field_2[0], CellState.Empty);
            Assert.AreEqual(field_2[1], CellState.Empty);
            Assert.AreEqual(field_2[2], CellState.Empty);
            Assert.AreEqual(field_2[3], CellState.Empty);

            var field_3 = new GameField(3);
            Assert.AreEqual(field_3.CellCount, 9);
            Assert.AreEqual(field_3[1], CellState.Empty);
            Assert.AreEqual(field_3[3], CellState.Empty);
            Assert.AreEqual(field_3[7], CellState.Empty);

            var field_4 = new GameField(4);
            Assert.AreEqual(field_4.CellCount, 16);
            Assert.AreEqual(field_4[1], CellState.BlackMan);
            Assert.AreEqual(field_4[4], CellState.Empty);
            Assert.AreEqual(field_4[9], CellState.Empty);
            Assert.AreEqual(field_4[12], CellState.WhiteMan);

            var field_8 = new GameField(8);
            Assert.AreEqual(field_8.CellCount, 64);
            Assert.AreEqual(field_8[1], CellState.BlackMan);
            Assert.AreEqual(field_8[8], CellState.BlackMan);
            Assert.AreEqual(field_8[17], CellState.BlackMan);
            Assert.AreEqual(field_8[24], CellState.Empty);
            Assert.AreEqual(field_8[33], CellState.Empty);
            Assert.AreEqual(field_8[40], CellState.WhiteMan);
            Assert.AreEqual(field_8[49], CellState.WhiteMan);
            Assert.AreEqual(field_8[56], CellState.WhiteMan);
        }

        [Test]
        public void TryCreateField_DefaultField()
        {
            var oldField = CommonValues.DefaultField;

            var correctTurns = new List<GameTurn> {
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 17, 26),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 23, 30),

                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White,46, 39),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White,42, 33),
            };

            var incorrectTurns = new List<GameTurn> {
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black,  7, 14),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 23, 24),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 24, 39),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 28, 35),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 42, 33),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 46, 39),

                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 17, 26),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 23, 30),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 33, 40),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 51, 44),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 51, 19),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 55, 40),
            };

            foreach (var turn in incorrectTurns)
            {
                Assert.IsFalse(GameFieldUtils.TryCreateField(oldField, turn, out _));
            }

            foreach (var turn in correctTurns)
            {
                Assert.IsTrue(GameFieldUtils.TryCreateField(oldField, turn, out GameField newField));

                foreach (var step in turn.Steps)
                {
                    Assert.AreNotEqual(oldField[step], newField[step]);
                }
            }
        }


        [Test]
        public void TryCreateField_NullTurn()
        {
            Assert.IsFalse(GameFieldUtils.TryCreateField(CommonValues.DefaultField, null, out _));
            Assert.IsFalse(GameFieldUtils.TryCreateField(CommonValues.ShortJumpsField, null, out _));

            Assert.IsFalse(GameFieldUtils.TryCreateField(CommonValues.DefaultField, null, out _));
            Assert.IsFalse(GameFieldUtils.TryCreateField(CommonValues.ShortJumpsField, null, out _));
        }

        [Test]
        public void TryCreateField_ShortJumpsField()
        {
            var oldField = CommonValues.ShortJumpsField;

            var correctTurns = new List<GameTurn> {
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 17, 35),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 19, 33),

                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 26, 12),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 24, 10),
            };

            var incorrectTurns = new List<GameTurn> {
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 17, 33),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 30, 12),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.Black, 58, 44),

                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 21, 35),
                GameTurnUtils.CreateTurnByTwoCells(oldField, PlayerSide.White, 21, 39),
            };

            foreach (var turn in incorrectTurns)
            {
                Assert.IsFalse(GameFieldUtils.TryCreateField(oldField, turn, out _));
            }

            foreach (var turn in correctTurns)
            {
                TestUtils.CheckTurnResult(oldField, turn);
            }
        }

        [Test]
        public void TryCreateField_LongJumpsField()
        {
            var oldField = CommonValues.LongJumpsField;

            var correctTurns = new List<GameTurn>{
                // PlayerSide.Black, new[] { 55, 46, 37, 30, 23, 14, 5, 12, 19 }
                TestUtils.CreateCompositeJump(oldField, PlayerSide.Black, new[] { 55, 37, 23, 5, 19 }),
                
                // PlayerSide.Black, new[] { 7, 14, 21, 30, 39, 46, 53 }
                TestUtils.CreateCompositeJump(oldField, PlayerSide.Black, new[] { 7, 21, 39, 53 }),
                
                // PlayerSide.White, new[] { 1, 10, 19, 28, 37, 44, 51, 42, 33 }
                TestUtils.CreateCompositeJump(oldField, PlayerSide.White, new[] { 1, 19, 37, 51, 33 })
            };

            var incorrectTurns = new List<GameTurn> {
                // PlayerSide.Black, new[] { 7, 14, 21, 28, 35, 42, 49 }
                TestUtils.CreateCompositeJump(oldField, PlayerSide.Black, new[] { 7, 21, 35, 49 }),
                
                // PlayerSide.Black, new[] {  7, 14, 21, 12, 3 }),
                TestUtils.CreateCompositeJump(oldField, PlayerSide.Black, new[] { 7, 21, 3 }),

                // PlayerSide.White, new[] {  1, 10, 19, 12, 5 }),
                TestUtils.CreateCompositeJump(oldField, PlayerSide.White, new[] { 1, 19, 5 }),

                // PlayerSide.White, new[] { 12, 10, 19, 28, 37, 44, 51, 42, 33 }),
                TestUtils.CreateCompositeJump(oldField, PlayerSide.White, new[] { 12, 19, 37, 51, 33})
            };


            foreach (var turn in incorrectTurns)
            {
                Assert.IsFalse(GameFieldUtils.TryCreateField(oldField, turn, out _));
            }

            foreach (var turn in correctTurns)
            {
                TestUtils.CheckTurnResult(oldField, turn);
            }
        }
    }
}
