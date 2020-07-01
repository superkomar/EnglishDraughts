﻿using System.Collections.Generic;

using EnglishDraughts.Model.Enums;

namespace EnglishDraughts.Model
{
    public class GameField
    {
        private readonly FieldCellState[] _field;

        public GameField(int dimension)
        {
            RowSize = dimension / 2;
            _field = new FieldCellState[RowSize * RowSize];

            var startPiecesCount = RowSize * 3;
            var lastCellIdx = _field.Length - 1;

            for (var idx = 0; idx < startPiecesCount; idx++)
            {
                _field[idx] = FieldCellState.BlackMen;
                _field[lastCellIdx - idx] = FieldCellState.WhiteMen;
            }
        }

        public IReadOnlyCollection<FieldCellState> Field => _field;

        public int RowSize { get; }
    }
}
