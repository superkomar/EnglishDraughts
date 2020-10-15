using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;
using Core.Properties;

namespace Core.Utils
{
    public static class GameFieldUtils
    {
        public static GameField CreateField(int dimension)
        {
            if (dimension <= 0) throw new ArgumentException(Resources.ArgumentException_Dimension);

            return new GameField(ConstructInitialField(dimension), new NeighborsFinder(dimension), dimension);
        }

        public static bool TryCreateField(GameField oldField, IGameTurn turn, out GameField newField)
        {
            newField = oldField;

            if (turn == null ||
                turn.IsSimple && GameTurnUtils.FindRequiredJumps(oldField, turn.Side).Any())
            {
                return false;
            }

            var newCells = new List<CellState>(oldField.Cells);
            var cellState = newCells[turn.Steps.First()];

            foreach (var step in turn.Steps.Take(turn.Steps.Count - 1))
            {
                newCells[step] = CellState.Empty;
            }

            newCells[turn.Steps.Last()] = turn.IsLevelUp ? cellState.LevelUp() : cellState;

            newField = new GameField(newCells, oldField);

            return true;
        }

        private static IReadOnlyList<CellState> ConstructInitialField(int dimension)
        {
            int rowCountWithPieces = dimension / 2 - 1;

            var initField = new List<CellState>();

            for (var i = 0; i < dimension; i++)
            {
                var startRowShift = i % 2;

                for (var j = 0; j < dimension; j++)
                {
                    if ((startRowShift + j) % 2 == 0)
                    {
                        initField.Add(CellState.Empty);
                    }
                    else if (i < rowCountWithPieces)
                    {
                        initField.Add(CellState.BlackMan);
                    }
                    else if (i > (dimension - rowCountWithPieces - 1))
                    {
                        initField.Add(CellState.WhiteMan);
                    }
                    else
                    {
                        initField.Add(CellState.Empty);
                    }
                }
            }

            return initField;
        }
    }
}
