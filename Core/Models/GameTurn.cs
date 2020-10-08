using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Interfaces;

namespace Core.Models
{
    public class GameTurn : IGameTurn
    {
        internal GameTurn(PlayerSide playerSide, bool isLevelUp, IReadOnlyList<int> turns)
        {
            Side = playerSide;
            IsLevelUp = isLevelUp;

            Steps = turns;
        }

        public bool IsSimple => Steps.Count == 2;

        public IReadOnlyList<int> Steps { get; }

        public bool IsLevelUp { get; }

        public PlayerSide Side { get; }

        public int Start => Steps.First();
    }
}
