using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Model;

namespace Core.Utils
{
    public static class GameFieldUpdater
    {
        public static bool TryMakeTurn(GameField oldGameField, GameTurn gameTurn, out GameField newGameField)
        {
            newGameField = oldGameField;

            if (gameTurn == null ||
                gameTurn.IsSimple && GameFieldUtils.FindRequiredJumps(oldGameField, gameTurn.Side).Any())
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

            newGameField = GameFieldUtils.GetNewField(oldGameField, newField);

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
    }
}
