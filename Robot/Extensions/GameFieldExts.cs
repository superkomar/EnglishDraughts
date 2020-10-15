using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

using Robot.Models;

namespace Robot.Extensions
{
    internal static class GameFieldExts
    {
        public static void ProcessAllCells(this GameField gameField, Action<int, bool> processor)
        {
            for (var i = 0; i < gameField.Dimension; i++)
            {
                for (var j = 0; j < gameField.Dimension; j++)
                {
                    var cellIdx = i * gameField.Dimension + j;
                    var isCellActive = (i + j) % 2 != 0;

                    processor(cellIdx, isCellActive);
                }
            }
        }

        public static void ProcessCellsBySide(this RobotField gameField, PlayerSide side, Action<int> processor)
        {
            foreach (var cellIdx in gameField.PiecesBySide(side))
            {
                processor(cellIdx);
            }
        }

        public static IEnumerable<IGameTurn> GetTurnsBySide(this RobotField gameField, PlayerSide side)
        {
            var simpleMoves = new List<IGameTurn>();
            var requiredJumps = new List<IGameTurn>();

            void Processor(int cellIdx)
            {
                var cellTurns = TurnUtils.FindTurnsForCell(gameField, cellIdx);

                foreach (var turn in cellTurns)
                {
                    if (turn.IsSimple) simpleMoves.Add(turn);
                    else requiredJumps.Add(turn);
                }
            }

            gameField.ProcessCellsBySide(side, Processor);

            return requiredJumps.Any() ? GetCompositeJumps(gameField.Origin, requiredJumps) : simpleMoves;
        }

        private static IEnumerable<IGameTurn> GetCompositeJumps(GameField oldField, IEnumerable<IGameTurn> jumps)
        {
            foreach (var jump in jumps)
            {
                FieldUtils.TryCreateField(oldField, jump, out GameField newField);
                var subJumps = GetCompositeJumps(newField, TurnUtils.FindTurnsForCell(newField, jump.Steps.Last(), TurnType.Jump));

                if (subJumps.Any() && !jump.IsLevelUp)
                {
                    foreach (var subJump in subJumps)
                    {
                        yield return TurnUtils.CreateCompositeTurn(new[] { jump, subJump });
                    }
                }
                else
                {
                    yield return jump;
                }
            }
        }
    }
}
