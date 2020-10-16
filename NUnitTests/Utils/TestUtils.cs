using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Models;
using Core.Utils;

using NUnit.Framework;

namespace NUnitTests.Utils
{
    internal static class TestUtils
    {
        public static void AreTurnsEqual(GameTurn first, GameTurn second)
        {
            Assert.NotNull(first);
            Assert.NotNull(second);

            Assert.AreEqual(first.Side, second.Side);
            Assert.AreEqual(first.IsSimple, second.IsSimple);
            Assert.AreEqual(first.IsLevelUp, second.IsLevelUp);
            Assert.AreEqual(first.Steps, second.Steps);
        }

        public static void CheckTurnResult(GameField oldField, GameTurn turn)
        {
            Assert.IsTrue(GameFieldUtils.TryCreateField(oldField, turn, out GameField newField));

            if (turn.IsLevelUp)
            {
                Assert.IsTrue(newField[turn.Steps.Last()].IsKing());
                Assert.IsFalse(oldField[turn.Steps.First()].IsKing());
            }
            else
            {
                Assert.AreEqual(oldField[turn.Steps.First()], newField[turn.Steps.Last()]);
            }

            Assert.AreNotEqual(oldField[turn.Steps.First()], newField[turn.Steps.First()]);
            Assert.AreNotEqual(oldField[turn.Steps.Last()],  newField[turn.Steps.Last()]);

            foreach (var step in turn.Steps.Skip(1).SkipLast(1))
            {
                Assert.AreEqual(CellState.Empty, newField[step]);
            }
        }

        public static void GetAllTurns(GameField field, out List<GameTurn> blackTurns, out List<GameTurn> whiteTurns)
        {
            var blackSimple = new List<GameTurn>();
            var blackJumps = new List<GameTurn>();

            var whiteSimple = new List<GameTurn>();
            var whiteJumps = new List<GameTurn>();

            static void Processor(IEnumerable<GameTurn> turns, IList<GameTurn> jumps, IList<GameTurn> simples)
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

                    var cellTurns = GameTurnUtils.FindTurnsForCell(field, cellIdx);

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

        public static GameTurn CreateCompositeJump(GameField startField, PlayerSide side, int[] turnsPoints)
        {
            var turns = new List<GameTurn>();

            var oldField = startField;

            for (var i = 0; i < turnsPoints.Length - 1; i++)
            {
                var start = turnsPoints[i];
                var end = turnsPoints[i + 1];

                turns.Add(GameTurnUtils.CreateTurnByTwoCells(oldField, side, start, end));

                if (!GameFieldUtils.TryCreateField(oldField, turns.Last(), out GameField newField))
                {
                    return null;
                }

                oldField = newField;
            }

            return GameTurnUtils.CreateCompositeJump(turns);
        }
    }
}
