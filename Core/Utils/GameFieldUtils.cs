using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;

namespace Core.Utils
{
    public static class GameFieldUtils
    {
        public static IEnumerable<GameTurn> FindRequiredJumps(GameField field, PlayerSide side)
        {
            for (var cellIdx = 0; cellIdx < field.Field.Count; cellIdx++)
            {
                var cellState = field[cellIdx];

                if (!cellState.IsSameSide(side)) continue;

                if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetLeftTopCell(cellIdx,  2), TurnType.Jump) is GameTurn leftTop)  yield return leftTop;
                if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetLeftBotCell(cellIdx,  2), TurnType.Jump) is GameTurn leftBot)  yield return leftBot;
                if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetRightTopCell(cellIdx, 2), TurnType.Jump) is GameTurn rightTop) yield return rightTop;
                if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetRightBotCell(cellIdx, 2), TurnType.Jump) is GameTurn rightBot) yield return rightBot;
            }
        }

        public static IEnumerable<GameTurn> FindTurnsForCell(GameField field, int cellIdx, TurnType type)
        {
            var cellState = field[cellIdx];

            if (!cellState.TryGetPlayerSide(out PlayerSide side)) yield break;
                 
            if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetLeftTopCell(cellIdx,  2), type) is GameTurn leftTop)  yield return leftTop;
            if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetLeftBotCell(cellIdx,  2), type) is GameTurn leftBot)  yield return leftBot;
            if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetRightTopCell(cellIdx, 2), type) is GameTurn rightTop) yield return rightTop;
            if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetRightBotCell(cellIdx, 2), type) is GameTurn rightBot) yield return rightBot;
        }

        public static bool TryMakeTurn(GameField oldGameField, IGameTurn gameTurn, out GameField newGameField)
        {
            newGameField = oldGameField;

            if (gameTurn == null ||
                gameTurn.IsSimple && FindRequiredJumps(oldGameField, gameTurn.Side).Any())
            {
                return false;
            }

            var newField = new List<CellState>(oldGameField.Field);
            var cellState = newField[gameTurn.Turns.First()];

            foreach (var turn in gameTurn.Turns.Take(gameTurn.Turns.Count - 1))
            {
                newField[turn] = CellState.Empty;
            }

            newField[gameTurn.Turns.Last()] = gameTurn.IsLevelUp ? cellState.LevelUp() : cellState;

            newGameField = GetNewField(oldGameField, newField);

            return true;
        }

        private static GameTurn GetTurn(GameField field, PlayerSide side, int startCellIdx, int endCellIdx, TurnType type = TurnType.Both) =>
            type switch
            {
                TurnType.Simple => ModelsCreator.CreateSimpleMove(field, side, startCellIdx, endCellIdx),
                TurnType.Jump   => ModelsCreator.CreateJumpMove(field, side, startCellIdx, endCellIdx),
                TurnType.Both   => GetTurn(field, side, startCellIdx, endCellIdx, TurnType.Simple)
                                   ?? GetTurn(field, side, startCellIdx, endCellIdx, TurnType.Jump),
                _ => throw new System.NotImplementedException()
            };

        internal static GameField GetNewField(GameField oldField, IReadOnlyList<CellState> newField) =>
            new GameField(newField, oldField.NeighborsHelper, oldField.Dimension);

        internal static int GetRowIdx(GameField field, int cellIdx) => cellIdx / field.Dimension;
    }
}
