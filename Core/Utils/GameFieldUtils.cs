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
        public static IEnumerable<GameTurn> FindRequiredJumps(GameField field, PlayerSide side)
        {
            for (var cellIdx = 0; cellIdx < field.Field.Count; cellIdx++)
            {
                var cellState = field[cellIdx];

                if (!cellState.IsSameSide(side)) continue;

                if (GetTurn(field, side, cellIdx, Direction.LeftTop,  TurnType.Jump) is GameTurn leftTop)  yield return leftTop;
                if (GetTurn(field, side, cellIdx, Direction.LeftBot,  TurnType.Jump) is GameTurn leftBot)  yield return leftBot;
                if (GetTurn(field, side, cellIdx, Direction.RightTop, TurnType.Jump) is GameTurn rightTop) yield return rightTop;
                if (GetTurn(field, side, cellIdx, Direction.RightBot, TurnType.Jump) is GameTurn rightBot) yield return rightBot;
            }
        }

        public static IEnumerable<GameTurn> FindTurnsForCell(GameField field, int cellIdx, TurnType type)
        {
            var cellState = field[cellIdx];

            if (!cellState.TryGetPlayerSide(out PlayerSide side)) yield break;
                 
            if (GetTurn(field, side, cellIdx, Direction.LeftTop,  type) is GameTurn leftTop)  yield return leftTop;
            if (GetTurn(field, side, cellIdx, Direction.LeftBot,  type) is GameTurn leftBot)  yield return leftBot;
            if (GetTurn(field, side, cellIdx, Direction.RightTop, type) is GameTurn rightTop) yield return rightTop;
            if (GetTurn(field, side, cellIdx, Direction.RightBot, type) is GameTurn rightBot) yield return rightBot;
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

        private static GameTurn GetTurn(GameField field, PlayerSide side, int startCellIdx, Direction direction, TurnType type = TurnType.Both)
        {
            int GetEndCellIdx(int deep) => field.NeighborsHelper.GetCellByDirection(startCellIdx, deep, direction);

            return type switch
            {
                TurnType.Simple => ModelsCreator.CreateSimpleMove(field, side, startCellIdx, GetEndCellIdx(1)),
                TurnType.Jump   => ModelsCreator.CreateJumpMove(field, side, startCellIdx, GetEndCellIdx(2)),
                TurnType.Both   => GetTurn(field, side, startCellIdx, direction, TurnType.Simple)
                                ?? GetTurn(field, side, startCellIdx, direction, TurnType.Jump),
                _ => default,
            };
        }
    }
}
