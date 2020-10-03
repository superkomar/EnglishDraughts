using System.ComponentModel;

using Core.Enums;

using Wpf.ViewModels.Enums;

namespace Wpf.Interfaces
{
    internal interface ICellHandler : INotifyPropertyChanged
    {
        int CellIdx { get; }
        
        CellColor CellColor { get; }

        CellState CellState { get; }
        
        bool IsEnabled { get; }

        bool IsSelected { get; set; }
    }
}
