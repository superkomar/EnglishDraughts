﻿using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Interfaces;
using Core.Models;

namespace Core.Utils
{
    public static class ModelsCreator
    {
        public static GameField CreateGameField(int dimension)
        {
            if (dimension <= 0) throw new ArgumentException(@"Fail! Dimention must be more then 0");

            return new GameField(ConstructInitialField(dimension), new NeighborsHelper(dimension), dimension);
        }

        public static GameTurn CreateGameTurn(GameField field, PlayerSide playerSide, int start, int end) =>
            field.NeighborsHelper.IsNeighbors(start, end)
                ? CreateSimpleMove(field, playerSide, start, end)
                : CreateJumpMove(field, playerSide, start, end);

        public static IGameTurn CreateGameTurn(IReadOnlyCollection<IGameTurn> turns)
        {
            if (!turns.Any() || turns.First() == null) return null;

            var result = new List<int>();
            IGameTurn lastTurn = null;

            foreach (var newTurn in turns)
            {
                if (newTurn == null || lastTurn != null && (lastTurn == newTurn
                    || lastTurn.Steps.Last() != newTurn.Steps.First()
                    || lastTurn.IsLevelUp))
                {
                    return null;
                }

                lastTurn = newTurn;
                result.AddRange(newTurn.Steps);
            }

            return new GameTurn(turns.First().Side, turns.Last().IsLevelUp, result);
        }

        public static GameTurn CreateJumpMove(GameField field, PlayerSide side, int start, int end)
        {
            if (!GameRules.IsMovePossible(field, side, start, end)) return null;

            var startNeighbours = field.NeighborsHelper[start];
            var endNeighbours = field.NeighborsHelper[end];

            var middle = startNeighbours.GetIntersection(endNeighbours);

            return middle != -1 && field[start].IsOpposite(field[middle])
                ? new GameTurn(side, GameRules.CanLevelUp(field, field[start], end), new[] { start, middle, end })
                : null;
        }

        public static GameTurn CreateSimpleMove(GameField field, PlayerSide side, int start, int end) =>
            GameRules.IsMovePossible(field, side, start, end)
                ? new GameTurn(side, GameRules.CanLevelUp(field, field[start], end), new[] { start, end })
                : null;

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
