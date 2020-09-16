using System.ComponentModel;

using Core.Enums;

using Wpf.ViewModels.Enums;

namespace Wpf.Interfaces
{
    internal interface ICellHandler : INotifyPropertyChanged
    {
        CellState CellState { get; }
        
        CellColor CellColor { get; }

        int CellIdx { get; }
        
        bool IsEnabled { get; }

        bool IsSelected { get; set; }
    }
}
