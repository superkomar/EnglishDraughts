using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Model;

namespace Core.Utils
{
    public static class ModelsCreator
    {
        public static GameField CreateGameField(int dimension)
        {
            if (dimension < 8) throw new ArgumentException(@"Field! Dimention must be more or equal then 8");

            return new GameField(ConstructInitialField(dimension), new NeighborsHelper(dimension), dimension);
        }

        public static GameTurn CreateGameTurn(GameField field, PlayerSide playerSide, int start, int end) =>
            field.NeighborsHelper.IsNeighbors(start, end)
                ? CreateSimpleMove(field, playerSide, start, end)
                : CreateJumpMove(field, playerSide, start, end);

        public static GameTurnCollection CreateGameTurnCollection(IReadOnlyList<GameTurn> turns)
        {
            if (!turns.Any()) return null;

            for (var i = 0; i < turns.Count - 1; i++)
            {
                var first = turns[i];
                var second = turns[i + 1];

                if (first == null || second == null || first.IsSimple || second.IsSimple
                    || first.Turns.Last() != second.Turns.First() || first.IsLevelUp)
                {
                    return null;
                }
            }

            return new GameTurnCollection(turns);
        }

        public static GameTurn CreateJumpMove(GameField field, PlayerSide side, int start, int end)
        {
            if (GameRules.IsMovePossible(field, side, start, end)) return null;

            var startNeighbours = field.NeighborsHelper[start];
            var endNeighbours = field.NeighborsHelper[end];

            var middle = -1;

            if (startNeighbours.LeftTop == endNeighbours.RightBot) middle = startNeighbours.LeftTop;
            else if (startNeighbours.LeftBot == endNeighbours.RightTop) middle = startNeighbours.LeftBot;
            else if (startNeighbours.RightTop == endNeighbours.LeftBot) middle = startNeighbours.RightTop;
            else if (startNeighbours.RightBot == endNeighbours.LeftTop) middle = startNeighbours.RightBot;

            return middle != -1 && field[start].IsOpposite(field[middle])
                ? new GameTurn(side, GameRules.CanLevelUp(field, end), new[] { start, middle, end })
                : null;
        }

        public static GameTurn CreateSimpleMove(GameField field, PlayerSide side, int start, int end) =>
            GameRules.IsMovePossible(field, side, start, end)
                ? new GameTurn(side, GameRules.CanLevelUp(field, end), new[] { start, end })
                : null;

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
