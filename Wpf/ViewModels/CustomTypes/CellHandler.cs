using Core.Enums;

using Wpf.Interfaces;
using Wpf.ViewModels.Enums;

namespace Wpf.ViewModels.CustomTypes
{
    public class CellHandler : NotifyPropertyChanged, ICellHandler
    {
        private readonly ISelectionController _selectionController;

        private CellState _cellState;
        private bool _isSelected;

        public CellHandler(int cellIdx, CellType cellType, CellState cellState, ISelectionController selectionController)
        {
            CellIdx = cellIdx;
            IsSelected = false;
            CellType = cellType;
            CellState = cellState;
            IsEnabled = cellType == CellType.Black;

            _selectionController = selectionController;
        }

        #region ICellHandler

        public int CellIdx { get; }

        public bool IsEnabled { get; }

        public CellType CellType { get; }

        public CellState CellState
        {
            get => _cellState;
            set => OnCellStateChanged(value);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => OnIsSelectedChanged(value);
        }

        #endregion

        private void OnCellStateChanged(CellState value)
        {
            if (value == _cellState) return;

            _cellState = value;
            OnPropertyChanged(nameof(CellState));
        }

        private void OnIsSelectedChanged(bool value)
        {
            if (_isSelected == value || !_selectionController.CanSelect(this))
            {
                return;
            }

            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }
}
