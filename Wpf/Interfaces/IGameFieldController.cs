namespace Wpf.Interfaces
{
    public interface IGameFieldController
    {
        int Dimension { get; }

        ICellHandler GetCellHandler(int posX, int posY);
    }
}
