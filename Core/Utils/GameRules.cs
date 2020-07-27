using Core.Enums;
using Core.Extensions;
using Core.Model;

namespace Core.Utils
{
    public static class GameRules
    {
        public static bool CanLevelUp(GameField field, int cellIdx) =>
            !field[cellIdx].IsKing() &&
            ((field[cellIdx].ToPlayerSide() == PlayerSide.Black && GameFieldUtils.GetRowIdx(field, cellIdx) == field.Dimension - 1) ||
             (field[cellIdx].ToPlayerSide() == PlayerSide.White && GameFieldUtils.GetRowIdx(field, cellIdx) == 0));

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
    }
}
