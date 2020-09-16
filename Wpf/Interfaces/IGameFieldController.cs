namespace Wpf.Interfaces
{
    internal interface IGameFieldController
    {
        int Dimension { get; }

        ICellHandler GetCellHandler(int posX, int posY);
    }
}
