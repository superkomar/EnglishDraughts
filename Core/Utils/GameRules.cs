using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Model;

namespace Core.Utils
{
    public static class GameRules
    {
        public static bool CanLevelUp(GameField field, CellState state, int end) =>
            !state.IsKing() &&
            ((state.ToPlayerSide() == PlayerSide.Black && GameFieldUtils.GetRowIdx(field, end) == field.Dimension - 1) ||
             (state.ToPlayerSide() == PlayerSide.White && GameFieldUtils.GetRowIdx(field, end) == 0));

        public static bool IsMovePossible(GameField field, PlayerSide side, int startIdx, int endIdx) =>
            startIdx != endIdx
            && IsValidTurnStart(field, startIdx) && IsValidTurnEnd(field, endIdx)
            && IsValidMoveDirection(field, startIdx, endIdx)
            && field[startIdx].ToPlayerSide() == side;

        public static bool IsValidCellIdx(GameField field, int cellIdx) =>
            cellIdx > 0 && cellIdx < field.Field.Count;
        
        public static bool IsValidTurnEnd(GameField field, int cellIdx) =>
            IsValidCellIdx(field, cellIdx) && field[cellIdx] == CellState.Empty;
        
        public static bool IsValidTurnStart(GameField field, int cellIdx) =>
            IsValidCellIdx(field, cellIdx) && field[cellIdx] != CellState.Empty;

        private static bool IsValidMoveDirection(GameField field, int start, int end) =>
            field[start].IsKing()
            || (field[start].ToPlayerSide() == PlayerSide.Black && start < end)
            || (field[start].ToPlayerSide() == PlayerSide.White && start > end);

        public static bool IsPlayerWin(GameField field, PlayerSide side)
        {
            var opposite = side.ToOpposite();
            return field.Field.Where(x => x.ToPlayerSide() == opposite).Any();
        }
    }
}
