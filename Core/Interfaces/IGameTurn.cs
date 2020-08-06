using System.Collections.Generic;

using Core.Enums;

namespace Core.Interfaces
{
    public interface IGameTurn
    {
        bool IsSimple { get; }

        bool IsLevelUp { get; }
        
        int Start { get; }

        PlayerSide Side { get; }

        IReadOnlyList<int> Turns { get; }
    }
}
