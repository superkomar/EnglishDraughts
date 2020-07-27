using System.Collections.Generic;

using Core.Enums;
using Core.Extensions;
using Core.Model;

namespace Core.Utils
{
    public static class GameFieldUtils
    {
        public static IEnumerable<GameTurn> FindRequiredJumps(GameField field, PlayerSide side)
        {
            for (var i = 0; i < field.Field.Count; i++)
            {
                var cellState = field[i];

                if (cellState == CellState.Empty || cellState.ToPlayerSide() != side) continue;

                if (ModelsCreator.CreateJumpMove(field, side, i, field.NeighborsHelper.GetLeftTopCell(i, 2))  is GameTurn leftTop)  yield return leftTop;
                if (ModelsCreator.CreateJumpMove(field, side, i, field.NeighborsHelper.GetLeftBotCell(i, 2))  is GameTurn leftBot)  yield return leftBot;
                if (ModelsCreator.CreateJumpMove(field, side, i, field.NeighborsHelper.GetRightTopCell(i, 2)) is GameTurn rightTop) yield return rightTop;
                if (ModelsCreator.CreateJumpMove(field, side, i, field.NeighborsHelper.GetRightBotCell(i, 2)) is GameTurn rightBot) yield return rightBot;
            }
        }

        internal static GameField GetNewField(GameField oldField, IReadOnlyList<CellState> newField) =>
            new GameField(newField, oldField.NeighborsHelper, oldField.Dimension);

        internal static int GetRowIdx(GameField field, int cellIdx) => cellIdx / field.Dimension;
    }
}
