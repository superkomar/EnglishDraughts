using System.Collections.Generic;

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

        public bool IsSimple => _turn.IsSimple;

        public PlayerSide Side => _turn.Side;

        public int Start => _turn.Start;

        public override string ToString()
        {
            var steps = IsSimple
                ? $"{_turn.Steps[0]} - {_turn.Steps[1]}"
                : $"{_turn.Steps[0]} - {_turn.Steps[1]} - {_turn.Steps[2]}";

            return $"{steps}; pr: {Priority:0.###};";
        }

        public IReadOnlyList<int> Steps => _turn.Steps;

        public void ClarifyPriority(double newValue, int level, bool isOpposite)
        {
            lock (_locker)
            {
                if (newValue == double.NegativeInfinity)
                {
                    Priority = newValue;
                }

                Priority += newValue * level * (isOpposite ? -1 : 1);
            }
        }

        public static explicit operator PriorityTurn(GameTurn turn) => new PriorityTurn(turn);
    }
}
