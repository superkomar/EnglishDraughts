using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Core.Enums;
using Core.Properties;

[assembly: InternalsVisibleTo("NUnitTests")]
namespace Core.Models
{
    public sealed class GameTurn
    {
        internal GameTurn(PlayerSide playerSide, bool isLevelUp, IReadOnlyList<int> steps)
        {
            if (steps == null || !steps.Any())
            {
                throw new ArgumentException(Resources.Error_EmptySteps);
            }

            Steps = steps;
            Side = playerSide;
            IsLevelUp = isLevelUp;
        }

        public bool IsSimple => Steps.Count == 2;

        public IReadOnlyList<int> Steps { get; }

        public bool IsLevelUp { get; }

        public PlayerSide Side { get; }

        public int Start => Steps.First();

        public override string ToString() => $"{Side} [{string.Join(" - ", Steps)}]";
    }
}
