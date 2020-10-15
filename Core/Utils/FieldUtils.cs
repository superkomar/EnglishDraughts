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
    public static class FieldUtils
    {
        public static GameField CreateField(int dimension)
        {
            if (dimension <= 0) throw new ArgumentException(Resources.ArgumentException_Dimension);

            return new GameField(ConstructInitialField(dimension), new NeighborsFinder(dimension), dimension);
        }

        public static bool TryCreateField(GameField oldField, IGameTurn gameTurn, out GameField newField)
        {
            newField = oldField;

            if (gameTurn == null ||
                gameTurn.IsSimple && TurnUtils.FindRequiredJumps(oldField, gameTurn.Side).Any())
            {
                return false;
            }

            var newCells = new List<CellState>(oldField.Field);
            var cellState = newCells[gameTurn.Steps.First()];

            foreach (var turn in gameTurn.Steps.Take(gameTurn.Steps.Count - 1))
            {
                newCells[turn] = CellState.Empty;
            }

            newCells[gameTurn.Steps.Last()] = gameTurn.IsLevelUp ? cellState.LevelUp() : cellState;

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
