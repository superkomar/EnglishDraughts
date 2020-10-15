using System;
using System.Collections.Generic;

using Core.Enums;
using Core.Extensions;
using Robot.Properties;

namespace Robot.Models
{
    internal interface ICompareByMetric
    {
        double MetricCost { get; }

        int Compare(RobotField oldField, RobotField newField, PlayerSide side);
    }

    internal class OwnPiecesCountMetric : ICompareByMetric
    {
        public double MetricCost => Settings.Default.OwnPiecesCountMetricCost;

        public int Compare(RobotField oldField, RobotField newField, PlayerSide side) =>
            oldField.PiecesCount(side) - newField.PiecesCount(side);
    }

    internal class EnemyPiecesCountMetric : ICompareByMetric
    {
        public double MetricCost => Settings.Default.EnemyPiecesCountMetricCost;

        public int Compare(RobotField oldField, RobotField newField, PlayerSide side)
        {
            var oppositeSide = side.ToOpposite();
            return oldField.PiecesCount(oppositeSide) - newField.PiecesCount(oppositeSide);
        }
    }

    internal class InvasionMetric : ICompareByMetric // Occupation
    {
        public double MetricCost => Settings.Default.InvasionMetricCost;

        public int Compare(RobotField oldField, RobotField newField, PlayerSide side)
        {
            int result = 0;

            var shift = side == PlayerSide.White ? oldField.Dimension - 1 : 0;

            for (var i = 0; i < oldField.Dimension; i++)
            {
                var lineIdx = Math.Abs(i - shift);

                result = GetPiecesOnLine(newField, side, lineIdx)
                    .CompareTo(GetPiecesOnLine(oldField, side, lineIdx));

                if (result != 0) break;
            }

            if (side == PlayerSide.Black)
            {
                for (var i = 0; i < oldField.Dimension; i++)
                {
                    result = GetPiecesOnLine(newField, side, i)
                        .CompareTo(GetPiecesOnLine(oldField, side, i));
                }
            }
            else
            {
                for (var i = oldField.Dimension - 1; i < 0; i--)
                {
                    result = GetPiecesOnLine(newField, side, i)
                        .CompareTo(GetPiecesOnLine(oldField, side, i));
                }
            }

            return result;
        }

        private int GetPiecesOnLine(RobotField field, PlayerSide side, int lineIdx)
        {
            var result = 0;

            var leftBorder = lineIdx * field.Dimension;
            var rightBorder = (lineIdx + 1) * field.Dimension;

            for (var i = leftBorder; i < rightBorder; i++)
            {
                if (field[i].IsSameSide(side))
                {
                    result++;
                }
            }

            return result;
        }
    }

    internal static class MetricsProcessor
    {
        private static readonly List<ICompareByMetric> _metrics =
            new List<ICompareByMetric> {
                new OwnPiecesCountMetric(),
                new EnemyPiecesCountMetric(),
                //new InvasionMetric()
            };

        public static double CompareWithMetrics(RobotField oldField, RobotField newField, PlayerSide side)
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
