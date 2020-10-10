using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;

namespace Core.Utils
{
    using Direction = NeighborsHelper.DirectionType;

    public static class GameFieldUtils
    {
        public static IEnumerable<IGameTurn> FindRequiredJumps(GameField field, PlayerSide side) =>
            field.Field.SelectMany((cell, idx) => cell.IsSameSide(side)
                ? FindTurnsForCell(field, idx, TurnType.Jump)
                : Enumerable.Empty<IGameTurn>());

        public static IEnumerable<IGameTurn> FindTurnsForCell(GameField field, int cellIdx, TurnType type) =>
            field[cellIdx].TryGetPlayerSide(out PlayerSide side)
                ? GetTurns(field, side, cellIdx, type)
                : Enumerable.Empty<IGameTurn>();

        public static IEnumerable<IGameTurn> FindTurnsForCell(GameField field, int cellIdx)
        {
            if (!field[cellIdx].TryGetPlayerSide(out PlayerSide side)) return Enumerable.Empty<IGameTurn>();

            var jumps = GetTurns(field, side, cellIdx, TurnType.Jump);

            return !jumps.Any()
                ? GetTurns(field, side, cellIdx, TurnType.Simple)
                : jumps;
        }

        public static bool TryMakeTurn(GameField oldGameField, IGameTurn gameTurn, out GameField newGameField)
        {
            newGameField = oldGameField;

            if (gameTurn == null ||
                gameTurn.IsSimple && FindRequiredJumps(oldGameField, gameTurn.Side).Any())
            {
                return false;
            }

            var newCells = new List<CellState>(oldGameField.Field);
            var cellState = newCells[gameTurn.Steps.First()];

            foreach (var turn in gameTurn.Steps.Take(gameTurn.Steps.Count - 1))
            {
                newCells[turn] = CellState.Empty;
            }

            newCells[gameTurn.Steps.Last()] = gameTurn.IsLevelUp ? cellState.LevelUp() : cellState;

            newGameField = new GameField(newCells, oldGameField);

            return true;
        }

        internal static int GetRowIdx(GameField field, int cellIdx) => cellIdx / field.Dimension;

        private static IEnumerable<IGameTurn> GetTurns(GameField field, PlayerSide side, int cellIdx, TurnType turnType)
        {
            if (CreateTurn(field, side, cellIdx, Direction.LeftTop,  turnType) is IGameTurn leftTop)  yield return leftTop;
            if (CreateTurn(field, side, cellIdx, Direction.LeftBot,  turnType) is IGameTurn leftBot)  yield return leftBot;
            if (CreateTurn(field, side, cellIdx, Direction.RightTop, turnType) is IGameTurn rightTop) yield return rightTop;
            if (CreateTurn(field, side, cellIdx, Direction.RightBot, turnType) is IGameTurn rightBot) yield return rightBot;
        }

        private static IGameTurn CreateTurn(GameField field, PlayerSide side, int startCellIdx, Direction direction, TurnType type)
        {
            int GetEndCellIdx(int deep) => field.NeighborsHelper.GetCellByDirection(startCellIdx, deep, direction);

            return type switch
            {
                TurnType.Simple => ModelsCreator.CreateSimpleMove(field, side, startCellIdx, GetEndCellIdx(1)),
                TurnType.Jump   => ModelsCreator.CreateJumpMove(field, side, startCellIdx, GetEndCellIdx(2)),
                _ => default,
            };
        }
    }
}
