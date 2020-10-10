using System;
using System.Collections.Generic;

using Core.Enums;
using Core.Extensions;

namespace Robot.Models
{
    internal interface ICompareByMetric
    {
        double MetricCost { get; }

        int Compare(FieldWrapper oldField, FieldWrapper newField, PlayerSide side);
    }

    internal class OwnPiecesCountMetric : ICompareByMetric
    {
        public double MetricCost => Constants.OwnPiecesCountMetricCost;

        public int Compare(FieldWrapper oldField, FieldWrapper newField, PlayerSide side) =>
            newField.PiecesCount(side).CompareTo(oldField.PiecesCount(side));
    }

    internal class EnemyPiecesCountMetric : ICompareByMetric
    {
        public double MetricCost => Constants.EnemyPiecesCountMetricCost;

        public int Compare(FieldWrapper oldField, FieldWrapper newField, PlayerSide side)
        {
            var oppositeSide = side.ToOpposite();
            return oldField.PiecesCount(oppositeSide).CompareTo(newField.PiecesCount(oppositeSide));
        }
    }

    internal class InvasionMetric : ICompareByMetric // Occupation
    {
        public double MetricCost => Constants.InvasionMetricCost;

        public int Compare(FieldWrapper oldField, FieldWrapper newField, PlayerSide side)
        {
            int result = 0;

            var shift = side == PlayerSide.Black ? oldField.Dimension - 1 : 0;

            for (var i = 0; i < oldField.Dimension; i++)
            {
                var lineIdx = Math.Abs(i - shift);

                result =
                    GetPiecesOnLine(newField, side, lineIdx).CompareTo(
                    GetPiecesOnLine(oldField, side, lineIdx));

                if (result != 0) break;
            }

            return result;
        }

        private int GetPiecesOnLine(FieldWrapper field, PlayerSide side, int lineIdx)
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
                new InvasionMetric()
            };

        public static double CompareByMetrics(FieldWrapper oldField, FieldWrapper newField, PlayerSide side)
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
