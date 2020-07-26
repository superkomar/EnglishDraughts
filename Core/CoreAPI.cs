using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Model;
using Core.Utils;

namespace Core
{
    public static class CoreAPI
    {
        public static GameField CreateGameField(int dimension)
        {
            if (dimension < 8) throw new ArgumentException(@"Field! Dimention must be more or equal then 8");

            return new GameField(ConstructInitialField(dimension), new NeighborsHelper(dimension), dimension);
        }

        public static GameTurn CreateGameTurn(GameField field, PlayerSide playerSide, int start, int end) =>
            field.NeighborsHelper.IsNeighbors(start, end)
                ? SimpleMove(field, playerSide, start, end)
                : JumpMove(field, playerSide, start, end);

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

        public static IEnumerable<GameTurn> FindRequiredJumps(GameField field, PlayerSide side)
        {
            for (var i = 0; i < field.Field.Count; i++)
            {
                var cellState = field[i];

                if (cellState.ToPlayerSide() != side) continue;

                if (JumpMove(field, side, i, field.NeighborsHelper.GetLeftBotCell(i, 2)) is GameTurn leftBot) yield return leftBot;
                if (JumpMove(field, side, i, field.NeighborsHelper.GetLeftTopCell(i, 2)) is GameTurn leftTop) yield return leftTop;
                if (JumpMove(field, side, i, field.NeighborsHelper.GetRightBotCell(i, 2)) is GameTurn rightBot) yield return rightBot;
                if (JumpMove(field, side, i, field.NeighborsHelper.GetRightTopCell(i, 2)) is GameTurn rightTop) yield return rightTop;
            }
        }

        public static GameTurn JumpMove(GameField field, PlayerSide side, int start, int end)
        {
            if (GameRules.CheckMovePossibility(field, side, start, end)) return null;

            var startNeighbours = field.NeighborsHelper[start];
            var endNeighbours = field.NeighborsHelper[end];

            var middle = -1;

            if      (startNeighbours.LeftTop == endNeighbours.RightBot) middle = startNeighbours.LeftTop;
            else if (startNeighbours.LeftBot == endNeighbours.RightTop) middle = startNeighbours.LeftBot;
            else if (startNeighbours.RightTop == endNeighbours.LeftBot) middle = startNeighbours.RightTop;
            else if (startNeighbours.RightBot == endNeighbours.LeftTop) middle = startNeighbours.RightBot;

            return middle != -1 && field[start].IsOpposite(field[middle])
                ? new GameTurn(side, GameRules.CanLevelUp(field, side, end), new[] { start, middle, end })
                : null;
        }

        public static GameTurn SimpleMove(GameField field, PlayerSide side, int start, int end) =>
            GameRules.CheckMovePossibility(field, side, start, end)
                ? new GameTurn(side, GameRules.CanLevelUp(field, side, end), new[] { start, end })
                : null;

        public static bool TryMakeTurn(GameField oldGameField, GameTurn gameTurn, out GameField newGameField)
        {
            newGameField = oldGameField;

            if (gameTurn == null ||
                gameTurn.IsSimple && FindRequiredJumps(oldGameField, gameTurn.Side).Any())
            {
                return false;
            }

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
