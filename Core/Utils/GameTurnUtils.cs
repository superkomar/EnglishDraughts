using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Models;

namespace Core.Utils
{
    using Direction = NeighborsFinder.DirectionType;

    public static class GameTurnUtils
    {
        public static GameTurn CreateCompositeJump(IReadOnlyCollection<GameTurn> turns)
        {
            if (!turns.Any() || turns.First() == null) return null;

            var result = new List<int>();

            GameTurn lastTurn = null;

            foreach (var newTurn in turns)
            {
                if (newTurn == null || lastTurn != null
                    && (lastTurn == newTurn
                    || lastTurn.Steps.Last() != newTurn.Steps.First()
                    || lastTurn.IsLevelUp))
                {
                    return null;
                }

                lastTurn = newTurn;
                result.AddRange(newTurn.Steps.SkipLast(1));
            }

            result.Add(turns.Last().Steps.Last());

            return new GameTurn(turns.First().Side, turns.Last().IsLevelUp, result);
        }

        public static GameTurn CreateTurnByTwoCells(GameField field, PlayerSide playerSide, int startIdx, int endIdx) =>
            field.NeighborsFinder.AreNeighbors(startIdx, endIdx)
                ? CreateSimpleTurn(field, playerSide, startIdx, endIdx)
                : CreateJumpTurn(field, playerSide, startIdx, endIdx);

        public static IEnumerable<GameTurn> FindRequiredJumps(GameField field, PlayerSide side) =>
            field.Cells.SelectMany(
                (cell, idx) => cell.IsSameSide(side)
                ? FindTurnsForCell(field, idx, TurnType.Jump)
                : Enumerable.Empty<GameTurn>());

        public static IEnumerable<GameTurn> FindTurnsForCell(GameField field, int cellIdx, TurnType type) =>
            field[cellIdx].TryGetPlayerSide(out PlayerSide side)
                ? GetTurnsForCell(field, side, cellIdx, type)
                : Enumerable.Empty<GameTurn>();

        public static IEnumerable<GameTurn> FindTurnsForCell(GameField field, int cellIdx)
        {
            var jumps = FindTurnsForCell(field, cellIdx, TurnType.Jump);
            return !jumps.Any()
                ? FindTurnsForCell(field, cellIdx, TurnType.Simple)
                : jumps;
        }

        private static GameTurn CreateJumpTurn(GameField field, PlayerSide side, int start, int end)
        {
            if (!GameRules.IsMovePossible(field, side, start, end)) return null;

            var startNeighbours = field.NeighborsFinder[start];
            var endNeighbours = field.NeighborsFinder[end];

            var middle = startNeighbours.GetIntersection(endNeighbours);

            return middle != -1 && field[start].IsOpposite(field[middle])
                ? new GameTurn(side, GameRules.CanLevelUp(field, field[start], end), new[] { start, middle, end })
                : null;
        }

        private static GameTurn CreateSimpleTurn(GameField field, PlayerSide side, int start, int end) =>
            GameRules.IsMovePossible(field, side, start, end)
                ? new GameTurn(side, GameRules.CanLevelUp(field, field[start], end), new[] { start, end })
                : null;
        
        private static GameTurn CreateTurnByDirection(GameField field, PlayerSide side, int startCellIdx, Direction direction, TurnType type)
        {
            int GetEndCellIdx(int deep) => field.NeighborsFinder.GetCellByDirection(startCellIdx, deep, direction);

            return type switch
            {
                TurnType.Simple => CreateSimpleTurn(field, side, startCellIdx, GetEndCellIdx(1)),
                TurnType.Jump => CreateJumpTurn(field, side, startCellIdx, GetEndCellIdx(2)),
                _ => throw new NotImplementedException(),
            };
        }

        private static IEnumerable<GameTurn> GetTurnsForCell(GameField field, PlayerSide side, int cellIdx, TurnType turnType)
        {
            if (CreateTurnByDirection(field, side, cellIdx, Direction.LeftTop,  turnType) is GameTurn leftTop)  yield return leftTop;
            if (CreateTurnByDirection(field, side, cellIdx, Direction.LeftBot,  turnType) is GameTurn leftBot)  yield return leftBot;
            if (CreateTurnByDirection(field, side, cellIdx, Direction.RightTop, turnType) is GameTurn rightTop) yield return rightTop;
            if (CreateTurnByDirection(field, side, cellIdx, Direction.RightBot, turnType) is GameTurn rightBot) yield return rightBot;
        }
    }
}
