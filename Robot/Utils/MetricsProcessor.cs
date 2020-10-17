using System.Collections.Generic;

using Core.Enums;
using Core.Extensions;

using Robot.Models;
using Robot.Properties;

namespace Robot.Utils
{
    internal interface ICompareByMetric
    {
        double MetricCost { get; }

        int Compare(CachedField oldField, CachedField newField, PlayerSide side);
    }

    internal class OwnPiecesCountMetric : ICompareByMetric
    {
        public double MetricCost => Settings.Default.OwnPiecesCountMetricCost;

        public int Compare(CachedField oldField, CachedField newField, PlayerSide side) =>
            oldField.PiecesCount(side) - newField.PiecesCount(side);
    }

    internal class EnemyPiecesCountMetric : ICompareByMetric
    {
        public double MetricCost => Settings.Default.EnemyPiecesCountMetricCost;

        public int Compare(CachedField oldField, CachedField newField, PlayerSide side)
        {
            var oppositeSide = side.ToOpposite();
            return oldField.PiecesCount(oppositeSide) - newField.PiecesCount(oppositeSide);
        }
    }

    internal static class MetricsProcessor
    {
        private static readonly List<ICompareByMetric> _metrics =
            new List<ICompareByMetric> {
                new OwnPiecesCountMetric(),
                new EnemyPiecesCountMetric(),
            };

        public static double CompareWithMetrics(CachedField oldField, CachedField newField, PlayerSide side)
        {
            double result = 0.0;

            foreach (var metric in _metrics)
            {
                result += metric.Compare(oldField, newField, side) * metric.MetricCost;
            }

            return result;
        }
    }
}
