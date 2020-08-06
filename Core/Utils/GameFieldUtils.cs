using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Model;


namespace Core.Utils
{
    public static class GameFieldUtils
    {
        public static IEnumerable<GameTurn> FindRequiredJumps(GameField field, PlayerSide side)
        {
            for (var cellIdx = 0; cellIdx < field.Field.Count; cellIdx++)
            {
                var cellState = field[cellIdx];

                if (cellState == CellState.Empty || cellState.ToPlayerSide() != side) continue;

                if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetLeftTopCell(cellIdx, 2), TurnType.Jump)  is GameTurn leftTop)  yield return leftTop;
                if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetLeftBotCell(cellIdx, 2), TurnType.Jump)  is GameTurn leftBot)  yield return leftBot;
                if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetRightTopCell(cellIdx, 2), TurnType.Jump) is GameTurn rightTop) yield return rightTop;
                if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetRightBotCell(cellIdx, 2), TurnType.Jump) is GameTurn rightBot) yield return rightBot;
            }
        }

        public static IEnumerable<GameTurn> FindTurnsForCell(GameField field, int cellIdx, TurnType type)
        {
            var cellState = field[cellIdx];

            if (cellState == CellState.Empty) yield break;

            var side = cellState.ToPlayerSide();

            if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetLeftTopCell(cellIdx, 2),  type) is GameTurn leftTop)  yield return leftTop;
            if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetLeftBotCell(cellIdx, 2),  type) is GameTurn leftBot)  yield return leftBot;
            if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetRightTopCell(cellIdx, 2), type) is GameTurn rightTop) yield return rightTop;
            if (GetTurn(field, side, cellIdx, field.NeighborsHelper.GetRightBotCell(cellIdx, 2), type) is GameTurn rightBot) yield return rightBot;
        }

        public enum TurnType
        {
            Simple,
            Jump,
            Both,
        }

        private static GameTurn GetTurn(GameField field, PlayerSide side, int startCellIdx, int endCellIdx, TurnType type = TurnType.Both) =>
            type switch
            {
                TurnType.Simple => ModelsCreator.CreateSimpleMove(field, side, startCellIdx, endCellIdx),
                TurnType.Jump => ModelsCreator.CreateJumpMove(field, side, startCellIdx, endCellIdx),
                TurnType.Both => GetTurn(field, side, startCellIdx, endCellIdx, TurnType.Simple) ?? GetTurn(field, side, startCellIdx, endCellIdx, TurnType.Jump),
                _ => throw new System.NotImplementedException()
            };

        internal static GameField GetNewField(GameField oldField, IReadOnlyList<CellState> newField) =>
            new GameField(newField, oldField.NeighborsHelper, oldField.Dimension);

        internal static int GetRowIdx(GameField field, int cellIdx) => cellIdx / field.Dimension;
    }
}
