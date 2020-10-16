using System.Collections.Generic;

using Core.Enums;
using Core.Models;
using Core.Utils;

using NUnit.Framework;

namespace NUnitTests.CoreTests
{
    [TestFixture]
    public class GameRulesTests
    {
        [Test]
        public void CanLevelUp()
        {
            var gameField = CommonValues.ShortJumpsField;

            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[1], 1));
            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[3], 3));
            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[7], 7));
            
            Assert.IsTrue(GameRules.CanLevelUp(gameField, gameField[5],  5));

            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[17], 17));
            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[19], 19));
            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[21], 21));
            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[23], 23));

            Assert.IsTrue(GameRules.CanLevelUp(gameField, gameField[56],  56));
            
            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[58], 58));
            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[60], 60));
            Assert.IsFalse(GameRules.CanLevelUp(gameField, gameField[62], 62));
        }

        [Test]
        public void IsValidCellIdx()
        {
            var gameField = CommonValues.ShortJumpsField;

            Assert.IsTrue(gameField.IsValidCellIdx(1));
            Assert.IsTrue(gameField.IsValidCellIdx(3));

            Assert.IsFalse(gameField.IsValidCellIdx(-1));
            Assert.IsFalse(gameField.IsValidCellIdx(gameField.Dimension * gameField.Dimension));
        }

        [Test]
        public void IsValidTurnEnd()
        {
            var gameField = CommonValues.ShortJumpsField;

            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, -1));

            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, 1));
            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, 3));
            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, 5));
            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, 7));

            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, 17));
            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, 19));
            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, 21));
            Assert.IsFalse(GameRules.IsValidTurnEnd(gameField, 23));

            Assert.IsTrue(GameRules.IsValidTurnEnd(gameField, 8));
            Assert.IsTrue(GameRules.IsValidTurnEnd(gameField, 10));
            Assert.IsTrue(GameRules.IsValidTurnEnd(gameField, 12));
            Assert.IsTrue(GameRules.IsValidTurnEnd(gameField, 14));
        }

        [Test]
        public void IsValidTurnStart()
        {
            var gameField = CommonValues.ShortJumpsField;

            Assert.IsFalse(GameRules.IsValidTurnStart(gameField, -1));

            Assert.IsTrue(GameRules.IsValidTurnStart(gameField, 1));
            Assert.IsTrue(GameRules.IsValidTurnStart(gameField, 3));
            Assert.IsTrue(GameRules.IsValidTurnStart(gameField, 5));
            Assert.IsTrue(GameRules.IsValidTurnStart(gameField, 7));

            Assert.IsTrue(GameRules.IsValidTurnStart(gameField, 17));
            Assert.IsTrue(GameRules.IsValidTurnStart(gameField, 19));
            Assert.IsTrue(GameRules.IsValidTurnStart(gameField, 21));
            Assert.IsTrue(GameRules.IsValidTurnStart(gameField, 23));

            Assert.IsFalse(GameRules.IsValidTurnStart(gameField, 8));
            Assert.IsFalse(GameRules.IsValidTurnStart(gameField, 10));
            Assert.IsFalse(GameRules.IsValidTurnStart(gameField, 12));
            Assert.IsFalse(GameRules.IsValidTurnStart(gameField, 14));
        }

        [Test]
        public void IsMovePossible()
        {
            var gameField = CommonValues.ShortJumpsField;

            // Incorrect
            foreach (var playerSide in new[] { PlayerSide.Black, PlayerSide.White })
            {
                Assert.IsFalse(GameRules.IsMovePossible(gameField, playerSide,  1, -1));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, playerSide, -1,  1));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, playerSide, -1, -1));
            }

            PlayerSide side;

            // Third row
            {
                side = PlayerSide.Black;

                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 17, 35));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 17, 8));
                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 19, 10));
                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 19, 12));
                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 19, 33));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 17, 10));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 17, 24));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 17, 26));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 19, 26));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 19, 28));

                side = PlayerSide.White;

                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 21, 12));
                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 21, 14));
                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 23, 14));
                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 23, 37));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 21, 28));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 21, 30));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 23, 30));
            }

            // Seventh row
            {
                side = PlayerSide.Black;

                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 51, 44));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 49, 40));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 49, 58));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 53, 44));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 51, 58));

                side = PlayerSide.White;

                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 53, 44));
                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 53, 46));
                Assert.IsTrue(GameRules.IsMovePossible(gameField, side, 55, 46));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 55, 62));
                Assert.IsFalse(GameRules.IsMovePossible(gameField, side, 51, 42));
            }
        }
    }
}
