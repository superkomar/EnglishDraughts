using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Wpf.Interfaces;

namespace Wpf.Views.Controls
{
    internal class GameCell : Grid, IDisposable
    {
        private readonly Border _border;
        private readonly ICellHandler _handler;
        private readonly Image _shapeImage;
        
        public GameCell(ICellHandler cellHandler, int width, int height)
        {
            Width = width;
            Height = height;

            _handler = cellHandler;
            IsEnabled = _handler?.IsEnabled ?? false;

            Children.Add(ControlUtils.ConstructImage(_handler.CellColor));

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

        public void Dispose()
        {
            DetachHandlers();
            Children.Clear();
        }

        private void AttachHandlers()
        {
            MouseUp += OnMouseUpChanged;
            _handler.PropertyChanged += OnHandlerPropertyChanged;
        }

        private void DetachHandlers()
        {
            MouseUp -= OnMouseUpChanged;
            _handler.PropertyChanged -= OnHandlerPropertyChanged;
        }

        private void OnHandlerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ICellHandler.CellState))
            {
                UpdateCellContent();
            }
            else if (e.PropertyName == nameof(ICellHandler.IsSelected))
            {
                SelectChanged(_handler.IsSelected);
            }
        }

        private void OnMouseUpChanged(object sender, MouseButtonEventArgs e)
        {
            _handler.IsSelected = !_handler.IsSelected;
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

        private void UpdateCellContent()
        {
            if (!IsEnabled) return;

            _shapeImage.Source = ControlUtils.GetBitmapByType(_handler.CellState);
        }
    }
}
