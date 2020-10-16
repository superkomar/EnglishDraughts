using System.Threading;

using Core.Models;

namespace Robot.Models
{
    internal class PriorityTurn
    {
        private double _priority;
        
        public PriorityTurn(GameTurn turn)
        {
            Origin = turn;
            _priority = 0.0;
        }
        
        public GameTurn Origin { get; }
        
        public double Priority => _priority;

        public static implicit operator GameTurn(PriorityTurn turn) => turn.Origin;

        public void ClarifyPriority(double additionalValue)
        {
            double localPriority;
            do
            {
                localPriority = _priority;
            } while (!CompareAndSwap(ref _priority, localPriority + additionalValue, localPriority));
        }

        public override string ToString() => $"{Origin}; pr: {Priority:0.###};";
        
        private static bool CompareAndSwap(ref double dst, double newValue, double oldValue) =>
            Interlocked.CompareExchange(ref dst, newValue, oldValue) == oldValue;
    }
}
