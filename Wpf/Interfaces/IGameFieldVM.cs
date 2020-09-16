using System;
using System.ComponentModel;

using Core.Model;

namespace Wpf.Interfaces
{
    internal interface IGameFieldVM : INotifyPropertyChanged
    {
        event EventHandler RedrawField;

        int Dimension { get; }

        public bool IsActive { get; }

        ICellHandler GetCellHandler(int posX, int posY);

        ICellHandler GetCellHandler(int cellIdx);

        void UpdateGameField(GameField gameField);
    }
}
