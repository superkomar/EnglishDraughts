using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Wpf.Interfaces;

namespace Wpf.Views.Controls
{
    public class GameCell : Grid
    {
        private readonly Border _border;
        private readonly Image _shapeImage;
        private readonly ICellHandler _handler;

        public GameCell(ICellHandler cellHandler, int width, int height)
        {
            Width = width;
            Height = height;

            _handler = cellHandler;
            IsEnabled = _handler?.IsEnabled ?? false;

            Children.Add(ControlUtils.ConstructImage(ControlUtils.GetBitmapByType(_handler.CellType)));

            _shapeImage = ControlUtils.ConstructImage(_handler.CellState);
            _shapeImage.Width = width * 0.6;
            _shapeImage.Height = Height * 0.6;
            Children.Add(_shapeImage);

            _border = new Border();
            Children.Add(_border);

            UpdateCellContent();

            SelectChanged(false);

            AttachHandlers();
        }

        private void UpdateCellContent()
        {
            if (!IsEnabled) return;

            _shapeImage.Source = ControlUtils.GetBitmapByType(_handler.CellState);
        }

        private void AttachHandlers()
        {
            MouseUp += OnMouseUpChanged;
            _handler.UpdateCellState += OnUpdateCellStateChanged;
        }

        private void OnUpdateCellStateChanged(object sender, EventArgs e)
        {
            UpdateCellContent();
        }

        private void SelectChanged(bool isSelect)
        {
            if (isSelect)
            {
                _border.BorderBrush = Brushes.Blue;
                _border.BorderThickness = new Thickness(3);
            }
            else
            {
                _border.BorderBrush = Brushes.Black;
                _border.BorderThickness = new Thickness(1);
            }
        }

        private void OnMouseUpChanged(object sender, MouseButtonEventArgs e)
        {
            _handler.MouseUp();

            SelectChanged(_handler.IsSelected);
        }
    }
}
