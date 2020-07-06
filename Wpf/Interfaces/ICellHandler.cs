using System;
using System.ComponentModel;

using Wpf.ViewModels.Enums;

namespace Wpf.Interfaces
{
    public interface ICellHandler : INotifyPropertyChanged
    {
        event EventHandler UpdateCellState;

        bool IsEnabled { get; }

        bool IsSelected { get; }

        CellType CellType { get; }

        CellState CellState { get; }

        void MouseUp();
    }
}
