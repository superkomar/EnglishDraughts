using System;
using System.Collections.Generic;
using System.ComponentModel;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;
using Wpf.ViewModels.Enums;

namespace Wpf.ViewModels
{
    public class GameFieldVM : NotifyPropertyChanged, IGameFieldVM
    {
        private readonly CustomObservableCollection<CellHandler> _cellHandlers;
        private readonly IList<CellHandler> _selectedCells;

        public GameFieldVM()
        {
            _cellHandlers = new CustomObservableCollection<CellHandler>();
            _selectedCells = new List<CellHandler>();

            AttacheHandlers();
        }

        private void AttacheHandlers()
        {
            _cellHandlers.ItemPropertyChanged += OnItemPropertyChanged;
        }

        public int Dimension => 8;

        public ICellHandler GetCellHandler(int posX, int posY)
        {
            var cellIdx = posX * Dimension + posY;
            var isCellActive = (posX + posY) % 2 != 0;

            var cellHandler = new CellHandler()
            {
                CellState = CellState.Empty,
                CellType = isCellActive ? CellType.Black : CellType.White,
                IsEnabled = isCellActive
            };

            if (isCellActive)
            {
                if (cellIdx < 8) cellHandler.CellState = CellState.BlackKing;
                else if (cellIdx < 24) cellHandler.CellState = CellState.BlackMen;

                if (cellIdx > 55) cellHandler.CellState = CellState.WhiteKing;
                else if (cellIdx > 39) cellHandler.CellState = CellState.WhiteMen;
            }

            _cellHandlers.Add(cellHandler);

            return cellHandler;
        }

        private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is CellHandler cellHandler)) return;

            if (cellHandler.IsSelected)
            {
                cellHandler.IsSelected = false;

                _selectedCells.Remove(cellHandler);
            }
            else if (_selectedCells.Count < 2)
            {
                cellHandler.IsSelected = true;

                _selectedCells.Add(cellHandler);
            }

            if (_selectedCells.Count == 2)
            {
                InverseField();

                foreach (var cell in _selectedCells)
                {
                    cell.IsSelected = false;
                }

                _selectedCells.Clear();
            }
        }

        private void InverseField()
        {
            foreach (var cell in _cellHandlers)
            {
                cell.CellState = cell.CellState switch
                {
                    CellState.BlackMen => CellState.WhiteMen,
                    CellState.BlackKing => CellState.WhiteKing,
                    CellState.WhiteMen => CellState.BlackMen,
                    CellState.WhiteKing => CellState.BlackKing,
                    _ => CellState.Empty,
                };
            }
        }

        private class CellHandler : ICellHandler
        {
            private CellState _cellState;
            private CellType _cellType;

            public event EventHandler UpdateCellState;

            public event PropertyChangedEventHandler PropertyChanged;

            public bool IsEnabled { get; set; }

            public bool IsSelected { get; set; }

            public CellType CellType
            {
                get => _cellType;
                set => OnCellTypeChanged(value);
            }

            public CellState CellState
            {
                get => _cellState;
                set => OnCellStateChanged(value);
            }

            public void MouseUp()
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }

            private void RaiseUpdateCell() => UpdateCellState?.Invoke(this, EventArgs.Empty);

            private void OnCellStateChanged(CellState value)
            {
                if (value == _cellState) return;

                _cellState = value;
                RaiseUpdateCell();
            }

            private void OnCellTypeChanged(CellType value)
            {
                if (value == _cellType) return;

                _cellType = value;
                RaiseUpdateCell();
            }
        }
    }
}
