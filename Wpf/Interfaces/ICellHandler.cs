using System.ComponentModel;

using Core.Enums;

using Wpf.ViewModels.Enums;

namespace Wpf.Interfaces
{
    public interface ICellHandler : INotifyPropertyChanged
    {
        Core.Enums.CellState CellState { get; }
        
        CellType CellType { get; }

        int CellIdx { get; }
        
        bool IsEnabled { get; }

        bool IsSelected { get; set; }
    }
}
