using System;

using Wpf.Interfaces;
using Wpf.ViewModels.Enums;

namespace Wpf.ViewModels.CustomTypes
{
    public class CellHandler : NotifyPropertyChanged, ICellHandler
    {
        private readonly Func<CellHandler, bool> _canSelect;

        private Core.Enums.CellState _cellState;
        private bool _isSelected;

        public CellHandler(int cellIdx, CellType cellType, Core.Enums.CellState cellState, Func<CellHandler, bool> canSelect = null)
        {
            CellIdx = cellIdx;
            IsSelected = false;
            CellType = cellType;
            CellState = cellState;
            IsEnabled = cellType == CellType.Black;

            _canSelect = canSelect ?? ((handler) => false);
        }

        #region ICellHandler

        public int CellIdx { get; }

        public bool IsEnabled { get; }

        public CellType CellType { get; }

        public Core.Enums.CellState CellState
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

        private void OnCellStateChanged(Core.Enums.CellState value)
        {
            if (value == _cellState) return;

            _cellState = value;
            OnPropertyChanged(nameof(CellState));
        }

        private void OnIsSelectedChanged(bool value)
        {
            if (_isSelected == value || !_canSelect(this))
            {
                return;
            }

            _isSelected = value;
            OnPropertyChanged(nameof(IsSelected));
        }
    }
}
