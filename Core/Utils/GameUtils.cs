using System.Collections.Generic;

using Core.Enums;
using Core.Model;

namespace Core.Utils
{
    internal static class GameUtils
    {
        public static int GetRowIdx(GameField field, int cellIdx) =>
            cellIdx / field.Dimension;

        public static GameField GetNewField(GameField oldField, IReadOnlyList<CellState> newField) =>
            new GameField(newField, oldField.FieldCache, oldField.Dimension);
    }
}
