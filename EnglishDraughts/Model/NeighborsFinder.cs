﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EnglishDraughts.Model
{
    public interface INeighborsFinder
    {
        IReadOnlyCollection<int> this[int cellId] { get; }
    }

    public class NeighborsFinder : INeighborsFinder
    {
        private readonly int _height;
        private readonly int _width;

        private readonly int[][] _neighbors;

        public NeighborsFinder(int fieldDimension)
        {
            if (fieldDimension <= 0) throw new ArgumentException(@"Field! Dimention must be more then 0");
            if (fieldDimension % 2 != 0) throw new ArgumentException(@"Field! Dimention must be multiple of two");

            _height = fieldDimension;
            _width = fieldDimension / 2;

            _neighbors = new int[_height * _width][];

            for (var i = 0; i < _neighbors.Length; i++)
            {
                var currRow = i / _width;
                var offset = i - ((currRow + 1) % 2);

                var cellNeighbors = new List<int>();

                if (CheckValue(currRow, offset - _width,     out int lt)) cellNeighbors.Add(lt);
                if (CheckValue(currRow, offset - _width + 1, out int rt)) cellNeighbors.Add(rt);
                if (CheckValue(currRow, offset + _width,     out int lb)) cellNeighbors.Add(lb);
                if (CheckValue(currRow, offset + _width + 1, out int rb)) cellNeighbors.Add(rb);

                _neighbors[i] = cellNeighbors.ToArray();
            }
        }

        public IReadOnlyCollection<int> this[int cellId] => _neighbors?.ElementAtOrDefault(cellId);

        private bool CheckValue(int currRow, int neigId, out int resId)
        {
            resId = neigId;

            return neigId >= 0 && neigId < _neighbors.Length && Math.Abs(currRow - neigId / _width) == 1;
        }
    }
}
