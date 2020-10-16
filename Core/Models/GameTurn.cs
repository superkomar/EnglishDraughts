using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Core.Enums;

[assembly: InternalsVisibleTo("NUnitTests")]
namespace Core.Models
{
    public sealed class GameTurn
    {
        internal GameTurn(PlayerSide playerSide, bool isLevelUp, IReadOnlyList<int> steps)
        {
            Steps = steps;
            Side = playerSide;
            IsLevelUp = isLevelUp;
        }

        public bool IsSimple => Steps.Count == 2;

        public IReadOnlyList<int> Steps { get; }

        public bool IsLevelUp { get; }

        public PlayerSide Side { get; }

        public int Start => Steps.First();

        public override string ToString() => $"[{string.Join(" - ", Steps)}]";
    }
}
