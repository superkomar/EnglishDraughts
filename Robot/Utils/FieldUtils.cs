using System;
using System.Collections.Generic;
using System.Text;

using Core.Enums;
using Core.Models;

using Robot.Models;

namespace Robot.Utils
{
    internal static class FieldUtils
    {
        public static void AllFieldProcessor(GameField gameField, Action<int, bool> processor)
        {
            for (var i = 0; i < gameField.Dimension; i++)
            {
                for (var j = 0; j < gameField.Dimension; j++)
                {
                    var cellIdx = i * gameField.Dimension + j;
                    var isCellActive = (i + j) % 2 != 0;

                    processor(cellIdx, isCellActive);
                }
            }
        }

        public static void ProcessorAllFieldBySide(FieldWrapper gameField, PlayerSide side, Action<int> processor)
        {
            foreach (var cellIdx in gameField.PiecesBySide(side))
            {
                processor(cellIdx);
            }
        }
    }
}
