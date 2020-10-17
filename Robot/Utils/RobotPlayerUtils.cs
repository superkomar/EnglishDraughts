using System.Collections.Generic;
using System.Linq;

using Core.Models;
using Core.Utils;

using Robot.Models;

namespace Robot.Utils
{
    internal class RobotPlayerUtils
    {
        public static IEnumerable<GameTurn> FindWorstTurn(CachedField oldField, IEnumerable<GameTurn> newTurns)
        {
            var index = -1;
            var minPriority = double.PositiveInfinity;

            foreach (var (turn, idx) in newTurns.Select((t, i) => (t, i)))
            {
                GameFieldUtils.TryCreateField(oldField.Origin, turn, out GameField newField);
                var turnMetric = MetricsProcessor.CompareWithMetrics(oldField, new CachedField(newField), turn.Side);

                if (turnMetric < minPriority)
                {
                    minPriority = turnMetric;
                    index = idx;
                }
            }

            return newTurns.Any() ? new[] { newTurns.ElementAt(index) } : newTurns;
        }
    }
}
