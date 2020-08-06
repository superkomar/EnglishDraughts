using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Interfaces;

namespace Core.Model
{
    public sealed class GameTurn : IGameTurn
    {
        internal GameTurn(PlayerSide playerSide, bool isLevelUp, IReadOnlyList<int> turns)
        {
            Side = playerSide;
            IsLevelUp = isLevelUp;

            Turns = turns;
        }

        public bool IsSimple => Turns.Count == 2;

        public IReadOnlyList<int> Turns { get; }

        public bool IsLevelUp { get; }

        public PlayerSide Side { get; }

        public int Start => Turns.First();
    }
}
