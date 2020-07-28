using System;
using System.ComponentModel;

namespace Wpf.Interfaces
{
    public interface IGameFieldVM : INotifyPropertyChanged
    {
        event EventHandler RedrawField;

        int Dimension { get; }

        ICellHandler GetCellHandler(int posX, int posY);

        ICellHandler GetCellHandler(int cellIdx);
    }
}
