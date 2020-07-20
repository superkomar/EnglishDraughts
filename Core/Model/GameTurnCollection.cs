using System.Collections.Generic;

namespace Core.Model
{
    public sealed class GameTurnCollection
    {
        internal GameTurnCollection(IReadOnlyList<GameTurn> turns)
        {
            Turns = turns;
        }

        public IReadOnlyList<GameTurn> Turns { get; }
    }
}
