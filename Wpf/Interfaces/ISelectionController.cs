namespace Wpf.Interfaces
{
    internal interface ISelectionController
    {
        bool CanSelect(ICellHandler cellHandler);

        void Clear();
    }
}
