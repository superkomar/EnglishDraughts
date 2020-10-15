using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Models;

namespace Core.Utils
{
    public static class GameRules
    {
        public static bool CanLevelUp(GameField field, CellState state, int end) =>
            !state.IsKing() &&
            ((state.IsSameSide(PlayerSide.Black) && field.GetRowIdx(end) == field.Dimension - 1) ||
             (state.IsSameSide(PlayerSide.White) && field.GetRowIdx(end) == 0));

        public static bool IsMovePossible(GameField field, PlayerSide side, int startIdx, int endIdx) =>
            startIdx != endIdx
            && IsValidTurnStart(field, startIdx) && IsValidTurnEnd(field, endIdx)
            && IsValidMoveDirection(field, startIdx, endIdx)
            && field[startIdx].IsSameSide(side);

        public static bool HasPlayerWon(GameField field, PlayerSide side)
        {
            var oppositeSide = side.ToOpposite();
            return !field.AreAnyPieces(oppositeSide)
                || !GameTurnUtils.FindTurnsForSide(field, oppositeSide).Any();
        }
        
        public static bool IsValidTurnEnd(GameField field, int cellIdx) =>
            field.IsValidCellIdx(cellIdx) && field[cellIdx] == CellState.Empty;
        
        public static bool IsValidTurnStart(GameField field, int cellIdx) =>
            field.IsValidCellIdx(cellIdx) && field[cellIdx] != CellState.Empty;

        private static bool IsValidMoveDirection(GameField field, int start, int end) =>
            field[start].IsKing()
            || (field[start].IsSameSide(PlayerSide.Black) && start < end)
            || (field[start].IsSameSide(PlayerSide.White) && start > end);
    }
}
