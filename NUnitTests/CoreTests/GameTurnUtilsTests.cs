using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Models;
using Core.Utils;

using NUnit.Framework;

using NUnitTests.Utils;

namespace NUnitTests.CoreTests
{
    [TestFixture]
    public class GameTurnUtilsTests
    {
        [Test]
        public void CreateCompositeTurn()
        {
            var gameField = CommonValues.LongJumpsField;

            var firstTurn = GameTurnUtils.CreateTurnByTwoCells(gameField, PlayerSide.Black, 55, 37);
            var compositeJump = TestUtils.CreateCompositeJump(
                gameField, PlayerSide.Black, new[] { 55, 37, 23,5, 19 });

            Assert.IsNull(GameTurnUtils.CreateCompositeJump(new GameTurn[0]));
            Assert.IsNull(GameTurnUtils.CreateCompositeJump(new GameTurn[] { null }));
            Assert.IsNull(GameTurnUtils.CreateCompositeJump(new GameTurn[] { null, null }));
            Assert.IsNull(GameTurnUtils.CreateCompositeJump(new GameTurn[] { firstTurn, null }));

            Assert.AreEqual(compositeJump.Steps, new[] { 55, 46, 37, 30, 23, 14, 5, 12, 19 });
        }


        [Test]
        public void FindRequiredJumps_DefaultField()
        {
            var gameField = CommonValues.DefaultField;

            Assert.AreEqual(GameTurnUtils.FindRequiredJumps(gameField, PlayerSide.Black).Count(), 0);
            Assert.AreEqual(GameTurnUtils.FindRequiredJumps(gameField, PlayerSide.White).Count(), 0);
        }

        [Test]
        public void FindRequiredJumps_ShortJumpsField()
        {
            var gameField = CommonValues.ShortJumpsField;

            var jumpsBlack = GameTurnUtils.FindRequiredJumps(gameField, PlayerSide.Black).ToArray();
            var jumpsWhite = GameTurnUtils.FindRequiredJumps(gameField, PlayerSide.White).ToArray();

            Assert.AreEqual(jumpsBlack.Length, 3);
            Assert.AreEqual(jumpsWhite.Length, 5);

            Assert.AreEqual(jumpsBlack[0].Steps, new int[] { 17, 26, 35 });
            Assert.AreEqual(jumpsBlack[1].Steps, new int[] { 19, 26, 33 });
            Assert.AreEqual(jumpsBlack[2].Steps, new int[] { 28, 21, 14 });

            Assert.AreEqual(jumpsWhite[0].Steps, new int[] { 23, 30, 37 });
            Assert.AreEqual(jumpsWhite[1].Steps, new int[] { 24, 17, 10 });
            Assert.AreEqual(jumpsWhite[2].Steps, new int[] { 26, 17, 8 });
            Assert.AreEqual(jumpsWhite[3].Steps, new int[] { 26, 19, 12 });
            Assert.AreEqual(jumpsWhite[4].Steps, new int[] { 60, 51, 42 });
        }

        [Test]
        public void FindRequiredJumps_LongJumpsField()
        {
            var gameField = CommonValues.LongJumpsField;

            var jumpsBlack = GameTurnUtils.FindRequiredJumps(gameField, PlayerSide.Black).ToList();
            var jumpsWhite = GameTurnUtils.FindRequiredJumps(gameField, PlayerSide.White).ToList();

            Assert.AreEqual(jumpsBlack.Count, 2);
            Assert.AreEqual(jumpsWhite.Count, 1);

            Assert.AreEqual(jumpsBlack[0].Steps, new int[] {  7, 14, 21 });
            Assert.AreEqual(jumpsBlack[1].Steps, new int[] { 55, 46, 37 });

            Assert.AreEqual(jumpsWhite[0].Steps, new int[] { 1, 10, 19 });
        }

        [Test]
        public void FindTurnsForCell_ShortJumpsField()
        {
            var gameField = CommonValues.ShortJumpsField;

            TestUtils.GetAllTurns(gameField,
                out List<GameTurn> blackTurns,
                out List<GameTurn> whiteTurns);

            // Black turns
            Assert.AreEqual(blackTurns.Count, 3);
            Assert.AreEqual(blackTurns[0].Steps, new int[] { 17, 26, 35 });
            Assert.AreEqual(blackTurns[1].Steps, new int[] { 19, 26, 33 });
            Assert.AreEqual(blackTurns[2].Steps, new int[] { 28, 21, 14 });

            // White turns
            Assert.AreEqual(whiteTurns.Count, 5);
            Assert.AreEqual(whiteTurns[0].Steps, new int[] { 23, 30, 37 });
            Assert.AreEqual(whiteTurns[1].Steps, new int[] { 24, 17, 10 });
            Assert.AreEqual(whiteTurns[2].Steps, new int[] { 26, 17, 8 });
            Assert.AreEqual(whiteTurns[3].Steps, new int[] { 26, 19, 12 });
            Assert.AreEqual(whiteTurns[4].Steps, new int[] { 60, 51, 42 });
        }

        [Test]
        public void FindTurnsForCell_DefaultField()
        {
            var gameField = CommonValues.DefaultField;

            var blackTurns = new List<GameTurn>(GameTurnUtils.FindRequiredJumps(gameField, PlayerSide.Black).ToArray());
            var whiteTurns = new List<GameTurn>(GameTurnUtils.FindRequiredJumps(gameField, PlayerSide.White).ToArray());

            Assert.AreEqual(blackTurns.Count, 0);
            Assert.AreEqual(whiteTurns.Count, 0);

            TestUtils.GetAllTurns(gameField, out blackTurns, out whiteTurns);

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
        public void CreateTurnByCells_ShortJumpsField()
        {
            var gameField = CommonValues.ShortJumpsField;

            PlayerSide curSide;

            // Black pieces
            curSide = PlayerSide.Black;

            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide,  1,  8));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide,  3, 12));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 19, 12));

            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 51, 58));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 17,  8));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 17, 26));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 19, 26));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 49, 56));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 26, 17));

            // White pieces
            curSide = PlayerSide.White;

            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 24, 33));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 53, 44));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 55, 46));

            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 24, 17));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 26, 17));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 26, 35));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 19, 12));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 53, 62));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 55, 62));
        }

        [Test]
        public void CreateTurnByCells_ShortJumpsField_IncorrectTurns()
        {
            var gameField = CommonValues.ShortJumpsField;

            foreach (var playerSide in new[] { PlayerSide.Black, PlayerSide.White })
            {
                Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, playerSide, -1, -1));
                Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, playerSide, -1, 1));
                Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, playerSide, 1, -1));
                Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, playerSide, 1, 1));
                Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, playerSide, 40, 33));
                Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, playerSide, 33, 40));
                Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, playerSide, 40, 39));
                Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, playerSide, 23, 24));
            }
        }

        [Test]
        public void CreateTurnByCells_ShortJumpsField_Jumps()
        {
            var gameField = CommonValues.ShortJumpsField;

            PlayerSide curSide;

            // Black pieces
            curSide = PlayerSide.Black;

            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 17, 35));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 19, 33));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 28, 14));

            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 30, 12));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 58, 40));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 60, 42));

            // White pieces
            curSide = PlayerSide.White;

            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 23, 37));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 24, 10));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 26, 12));
            Assert.IsNotNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 60, 42));

            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 21, 39));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 21, 35));
            Assert.IsNull(GameTurnUtils.CreateTurnByTwoCells(gameField, curSide, 62, 44));
        }
    }
}
