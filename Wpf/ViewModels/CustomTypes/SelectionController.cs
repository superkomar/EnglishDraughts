using Core.Enums;
using Core.Extensions;

namespace Wpf.ViewModels.CustomTypes
{
    internal interface ISelectionController
    {
        bool CanSelect(CellHandler cellHandler);

        void Clear();
    }

    internal class SelectionController : ISelectionController
    {
        private readonly ITurnsController _turnController;

        private CellHandler _lastSelectedCell;

        public SelectionController(ITurnsController turnsController)
        {
            _turnController = turnsController;
        }

        public bool CanSelect(CellHandler cellHandler)
        {
            if (cellHandler == null)
            {
                return false;
            }

            // Deselect cell
            if (_lastSelectedCell == cellHandler)
            {
                _lastSelectedCell = null;
                return true;
            }

            if (_lastSelectedCell == null)
            {
                if (_turnController.CheckTurnStartCell(cellHandler.CellIdx) &&
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
    }
}
