using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Wpf.Interfaces;

namespace Wpf.Views.Controls
{
    internal class GameField : Grid
    {
        public GameField()
        {
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
        }

        public void GenerateNewField(IGameFieldVM fieldController)
        {
            if (fieldController == null) return;

            for (int i = 0; i < fieldController.Dimension; i++)
            {
                RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            for (int j = 0; j < fieldController.Dimension; j++)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            Margin = new Thickness(0);
            Background = Brushes.Black;

            for (int i = 0; i < fieldController.Dimension; i++)
            {
                for (int j = 0; j < fieldController.Dimension; j++)
                {
                    var cell = new GameCell(fieldController.GetCellHandler(i, j), 60, 60)
                    {
                        Margin = new Thickness(0),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };

                    Children.Add(cell);

                    SetColumn(cell, j);
                    SetRow(cell, i);
                }
            }
        }

        public void ResetField()
        {
            foreach (var cell in Children.Cast<IDisposable>())
            {
                cell.Dispose();
            }

            Children.Clear();
        }
    }
}
