using System.Collections.Generic;

using Core.Enums;

namespace Core.Interfaces
{
    public interface IGameTurn
    {
        bool IsLevelUp { get; }
        
        bool IsSimple { get; }
        
        PlayerSide Side { get; }
        
        int Start { get; }
        
        IReadOnlyList<int> Steps { get; }
    }
}
