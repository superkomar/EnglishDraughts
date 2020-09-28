using System;
using System.ComponentModel;

using Core.Models;

namespace Wpf.Interfaces
{
    internal interface IGameFieldVM : INotifyPropertyChanged
    {
        event EventHandler RedrawField;

        int Dimension { get; }

        ICellHandler GetCellHandler(int posX, int posY);

        ICellHandler GetCellHandler(int cellIdx);

        void UpdateGameField(GameField gameField);
    }
}
