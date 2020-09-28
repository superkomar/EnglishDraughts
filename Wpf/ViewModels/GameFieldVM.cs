using System;
using System.Linq;

using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;
using Wpf.ViewModels.Enums;

namespace Wpf.ViewModels
{
    interface IWpfTurnWaiter
    {
        void Start(GameField newField, PlayerSide side, IStatusReporter reporter, IResultSetter<IGameTurn> sender);

        void Stop();
    }

    internal class GameFieldVM : ViewModelBase, IGameFieldVM, IWpfTurnWaiter
    {
        private readonly CustomObservableCollection<CellHandler> _cellHandlers; // ???
        private readonly SelectionController _selectionController;
        private readonly TurnsConstructor _turnsController;

        public GameFieldVM()
        {
            _cellHandlers = new CustomObservableCollection<CellHandler>();

            _turnsController = new TurnsConstructor();
            _selectionController = new SelectionController(_turnsController);

            UpdateGameField(ModelsCreator.CreateGameField(Dimension));
        }

        #region IGameFieldVM

        public event EventHandler RedrawField;

        public int Dimension { get; private set; } = Constants.FieldDimension;

        public ICellHandler GetCellHandler(int posX, int posY) => GetCellHandler(posX * Dimension + posY);

        public ICellHandler GetCellHandler(int cellIdx) => _cellHandlers.ElementAtOrDefault(cellIdx);

        #endregion

        #region IWpfTurnWaiter

        public void Start(GameField newField, PlayerSide side, IStatusReporter reporter, IResultSetter<IGameTurn> resultSetter)
        {
            _selectionController.IsSelectionAvaliable = true;
            _turnsController.UpdateState(newField, side, reporter, resultSetter);
        }

        public void Stop() => _selectionController.IsSelectionAvaliable = false;

        #endregion

        public void UpdateGameField(GameField gameField)
        {

            _selectionController.Clear();

            void updateAction(int idx, CellHandler handler)
            {
                if ((_cellHandlers.Count - 1) > idx)
                {
                    _cellHandlers[idx].CellState = handler.CellState;
                }
                else _cellHandlers.Add(handler);
            }

            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var cellIdx = i * Dimension + j;
                    var isCellActive = (i + j) % 2 != 0;

                    var cellHandler = new CellHandler(
                        cellIdx,
                        isCellActive ? CellColor.Black : CellColor.White,
                        gameField[cellIdx],
                        _selectionController);

                    updateAction(cellIdx, cellHandler);
                }
            }
        }
    }
}
