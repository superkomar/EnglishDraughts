using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Robot.Models
{
    internal class PriorityTurn : IGameTurn
    {
        private readonly object _locker = new object();

        private readonly IGameTurn _turn;

        public PriorityTurn(IGameTurn turn)
        {
            _turn = turn;
            Priority = 0.0;
        }

        public double Priority { get; private set; }

        public bool IsLevelUp => _turn.IsLevelUp;

        public bool IsSimple => _turn.IsLevelUp;

        public PlayerSide Side => _turn.Side;

        public int Start => _turn.Start;

        public IReadOnlyList<int> Steps => _turn.Steps;

        public void ClarifyPriority(double newValue, int level, PlayerSide side)
        {
            lock (_locker)
            {
                if (newValue == double.NegativeInfinity)
                {
                    Priority = newValue;
                }

                if (side == Side) Priority += newValue * level;
                else Priority -= newValue * level;
            }
        }

        public static explicit operator PriorityTurn(GameTurn turn) => new PriorityTurn(turn);
    }

    internal class PriorityTurnCollection : List<PriorityTurn>
    {
        public PriorityTurnCollection(List<GameTurn> turns)
            : base(turns.Select(x => new PriorityTurn(x)))
        { }

        public IGameTurn GetBestTurn()
        {
            var result = this.First();

            foreach (var turn in this.Skip(1))
            {
                if (result.Priority < turn.Priority)
                {
                    result = turn;
                }
            }

            return result;
        }
    }
}
