using Core.Enums;
using Core.Extensions;
using Core.Model;

namespace Core.Utils
{
    public static class GameRules
    {
        public static bool CanLevelUp(GameField field, PlayerSide side, int cellIdx) =>
            !field[cellIdx].IsKing() &&
            ((side == PlayerSide.Black && GameUtils.GetRowIdx(field, cellIdx) == field.Dimension - 1) ||
             (side == PlayerSide.White && GameUtils.GetRowIdx(field, cellIdx) == 0));

        public static bool CheckCellIdx(GameField field, int cellIdx) =>
            cellIdx > 0 && cellIdx < field.Field.Count;

        public static bool CheckEndCell(GameField field, int cellIdx) =>
            CheckCellIdx(field, cellIdx) && field[cellIdx] == CellState.Empty;

        public static bool CheckMovePossibility(GameField field, PlayerSide side, int startIdx, int endIdx) =>
            startIdx != endIdx && IsMoveDirection(field, startIdx, endIdx)
            && CheckStartCell(field, startIdx) && CheckEndCell(field, endIdx)
            && field[startIdx].ToPlayerSide() == side;

        public static bool CheckStartCell(GameField field, int cellIdx) =>
            CheckCellIdx(field, cellIdx) && field[cellIdx] != CellState.Empty;

        public static bool IsMoveDirection(GameField field, int cellIdx, int dirIdx) =>
            field[cellIdx].IsKing()
            || (field[cellIdx].ToPlayerSide() == PlayerSide.Black && cellIdx < dirIdx)
            || (field[cellIdx].ToPlayerSide() == PlayerSide.White && cellIdx > dirIdx);
    }
}
