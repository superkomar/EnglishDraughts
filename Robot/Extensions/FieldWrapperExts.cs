using System;

using Core.Enums;

using Robot.Models;

namespace Robot.Extensions
{
    internal static class FieldWrapperExts
    {
        public static void ProcessorCellBySide(this FieldWrapper gameField, PlayerSide side, Action<int> processor)
        {
            foreach (var cellIdx in gameField.PiecesBySide(side))
            {
                processor(cellIdx);
            }
        }
    }
}
