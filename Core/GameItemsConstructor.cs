using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Model;
using Core.Utils;

namespace Core
{
    public static class GameItemsConstructor
    {
        public static GameField NewGameField(int dimension)
        {
            if (dimension < 8) throw new ArgumentException(@"Field! Dimention must be more or equal then 8");

            return new GameField(ConstructInitialField(dimension), new GameFieldCache(dimension), dimension);
        }

        public static GameTurn NewGameTurn(GameField field, PlayerSide playerSide, int start, int end)
        {
            if (!GameRules.CheckMoveCells(field, playerSide, start, end) || !GameRules.IsValidMoveDirection(field, start, end))
            {
                return null;
            }

            if (field.FieldCache.IsNeighbors(start, end))
            {
                return new GameTurn(playerSide, GameRules.IsCellLevelUp(field, playerSide, end), new[] { start, end });
            }
            else
            {
                var startNeighbours = field.FieldCache[start];
                var endNeighbours = field.FieldCache[end];

                var middle = -1;

                if      (startNeighbours.LeftTop == endNeighbours.RightBot) middle = startNeighbours.LeftTop;
                else if (startNeighbours.LeftBot == endNeighbours.RightTop) middle = startNeighbours.LeftBot;
                else if (startNeighbours.RightTop == endNeighbours.LeftBot) middle = startNeighbours.RightTop;
                else if (startNeighbours.RightBot == endNeighbours.LeftTop) middle = startNeighbours.RightBot;

                return middle != -1 && field[start].IsOpposite(field[middle])
                    ? new GameTurn(playerSide, GameRules.IsCellLevelUp(field, playerSide, end), new[] { start, middle, end })
                    : null;
            }
        }

        public static GameTurnCollection NewTurnCollection(IReadOnlyList<GameTurn> turns)
        {
            if (!turns.Any()) return null;

            for (var i = 0; i < turns.Count - 1; i++)
            {
                var first = turns[i];
                var second = turns[i + 1];

                if (first == null || second == null
                    || first.Turns.Last() != second.Turns.First()
                    || first.IsLevelUp)
                {
                    return null;
                }
            }

            return new GameTurnCollection(turns);
        }

        public static bool TryMakeTurn(GameField oldGameField, GameTurn gameTurn, out GameField newGameField)
        {
            newGameField = oldGameField;

            if (gameTurn == null) return false;

            var newField = new List<CellState>(oldGameField.Field);
            var cellState = newField[gameTurn.Turns.First()];

            foreach (var turn in gameTurn.Turns.Take(gameTurn.Turns.Count - 1))
            {
                newField[turn] = CellState.Empty;
            }

            newField[gameTurn.Turns.Last()] = gameTurn.IsLevelUp ? cellState.LevelUp() : cellState;

            newGameField = GameUtils.GetNewField(oldGameField, newField);

            return true;
        }

        public static bool TryMakeTurns(GameField oldField, GameTurnCollection turns, out GameField newField)
        {
            newField = oldField;

            for (var i = 0; i < turns.Turns.Count; i++)
            {
                if (!TryMakeTurn(newField, turns.Turns[i], out GameField newLocalField))
                {
                    return false;
                }

                newField = newLocalField;
            }

            return true;
        }

        private static IReadOnlyList<CellState> ConstructInitialField(int dimension)
        {
            const int rowCountWithPieces = 3;

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
                        initField.Add(CellState.BlackMen);
                    }
                    else if (i > (dimension - rowCountWithPieces - 1))
                    {
                        initField.Add(CellState.WhiteMen);
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
