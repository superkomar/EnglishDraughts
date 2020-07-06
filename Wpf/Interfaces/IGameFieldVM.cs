using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Wpf.Interfaces
{
    public interface IGameFieldVM : INotifyPropertyChanged
    {
        int Dimension { get; }

        ICellHandler GetCellHandler(int posX, int posY);
    }
}
