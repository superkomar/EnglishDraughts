using Core.Enums;
using Core.Utils;

namespace Core.Model
{
    internal static class GameRules
    {
        public static bool CheckMoveCells(GameField field, PlayerSide playerSide, int startIdx, int endIdx) =>
            startIdx != endIdx && CheckStartCell(field, startIdx) && CheckEndCell(field, endIdx) && field[startIdx].ToPlayerSide() == playerSide;

        public static bool IsCellLevelUp(GameField field, PlayerSide playerSide, int cellIdx) =>
            !field[cellIdx].IsKing() &&
            ((playerSide == PlayerSide.Black && GameUtils.GetRowIdx(field, cellIdx) == field.Dimension - 1) ||
             (playerSide == PlayerSide.White && GameUtils.GetRowIdx(field, cellIdx) == 0));

        public static bool IsValidMoveDirection(GameField field, int cellIdx, int dirIdx) =>
            field[cellIdx].IsKing()
            || (field[cellIdx].ToPlayerSide() == PlayerSide.Black && cellIdx < dirIdx)
            || (field[cellIdx].ToPlayerSide() == PlayerSide.White && cellIdx > dirIdx);

        private static bool CheckCellIdx(GameField field, int cellIdx) =>
            cellIdx > 0 && cellIdx < field.Field.Count;

        private static bool CheckEndCell(GameField field, int cellIdx) =>
            CheckCellIdx(field, cellIdx) && field[cellIdx] == CellState.Empty;

        private static bool CheckStartCell(GameField field, int cellIdx) =>
            CheckCellIdx(field, cellIdx) && field[cellIdx] != CellState.Empty;
    }
}
