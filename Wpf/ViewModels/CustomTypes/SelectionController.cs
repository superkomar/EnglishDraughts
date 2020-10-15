using Core.Enums;
using Core.Extensions;

using Wpf.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    internal class SelectionController : ISelectionController
    {
        private readonly ITurnsConstructor _turnController;

        private ICellHandler _lastSelectedCell;

        public SelectionController(ITurnsConstructor turnsController)
        {
            _turnController = turnsController;
        }

        public bool IsSelectionAvaliable { get; set; }

        #region ISelectionController

        public bool CanSelect(ICellHandler cellHandler)
        {
            if (cellHandler == null) return false;

            // Deselect cell
            if (_lastSelectedCell == cellHandler)
            {
                _lastSelectedCell = null;
                return true;
            }

            if (!IsSelectionAvaliable) return false;

            if (_lastSelectedCell == null)
            {
                if (_turnController.CheckTurnStart(cellHandler.CellIdx) &&
                    cellHandler.CellState.IsSameSide(_turnController.Side))
                {
                    _lastSelectedCell = cellHandler;
                    return true;
                }

                return false;
            }

            if (cellHandler.CellState == CellState.Empty)
            {
                var result = _turnController.TryMakeTurn(_lastSelectedCell.CellIdx, cellHandler.CellIdx);

                switch (result)
                {
                    case TurnsConstructor.Result.Fail:
                    {
                        return false;
                    }
                    case TurnsConstructor.Result.Ok:
                    {
                        if (_lastSelectedCell != null)
                        {
                            _lastSelectedCell.IsSelected = false;
                            _lastSelectedCell = null;
                        }

                        return false;
                    }
                    case TurnsConstructor.Result.Continue:
                    {
                        _lastSelectedCell.IsSelected = false;
                        _lastSelectedCell = cellHandler;
                        return true;
                    }
                }
            }

            return false;
        }

        public void Clear()
        {
            if (_lastSelectedCell != null)
            {
                _lastSelectedCell.IsSelected = false;
                _lastSelectedCell = null;

                _turnController.Clear();
            }
        }

        #endregion
    }
}
