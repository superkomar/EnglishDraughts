using System;

namespace Wpf.Interfaces
{
    internal interface ICellHandlersController
    {
        event EventHandler RedrawFieldEvent;

        int Dimension { get; }

        ICellHandler GetCellHandler(int posX, int posY);

        ICellHandler GetCellHandler(int cellIdx);
    }
}
