using System.Collections.Generic;
using System.Threading;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Robot.Models
{
    internal class PriorityTurn : IGameTurn
    {
        private readonly IGameTurn _turn;

        public PriorityTurn(IGameTurn turn)
        {
            _turn = turn;
            _priority = 0.0;
        }

        private double _priority;

        public double Priority => _priority;

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

        public void ClarifyPriority(double additionalValue)
        {
            double localPriority;
            do
            {
                localPriority = _priority;
            } while (!CompareAndSwap(ref _priority, localPriority + additionalValue, localPriority));
        }

        private static bool CompareAndSwap(ref double dst, double newValue, double oldValue) =>
            Interlocked.CompareExchange(ref dst, newValue, oldValue) == oldValue;

        public static explicit operator PriorityTurn(GameTurn turn) => new PriorityTurn(turn);
    }
}
