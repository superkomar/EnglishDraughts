using System.Threading;

namespace Robot.Models
{
    internal readonly struct EstimationParameters
    {
        public EstimationParameters(PriorityTurn generanTurn, int depth, CancellationToken token)
        {
            Depth = depth;
            Token = token;
            TargetTurn = generanTurn;
        }

        public EstimationParameters(EstimationParameters other, int level)
        {
            Depth = level;

            Token = other.Token;
            TargetTurn = other.TargetTurn;
        }

        public int Depth { get; }

        public PriorityTurn TargetTurn { get; }

        public CancellationToken Token { get; }
    }
}
