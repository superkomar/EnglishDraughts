using System;
using System.Collections.Generic;
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
    internal class GameFieldVM : NotifyPropertyChanged, ICellHandlersController, IWpfFieldActivator
    {
        private readonly List<CellHandler> _cellHandlers;
        private readonly SelectionController _selectionController;
        private readonly TurnsConstructor _turnsController;

        public GameFieldVM()
        {
            _cellHandlers = new List<CellHandler>();

            _turnsController = new TurnsConstructor();
            _selectionController = new SelectionController(_turnsController);

            InitGameField(ModelsCreator.CreateGameField(Dimension));
        }

        #region ICellHandlersController

        public event EventHandler RedrawField;

        public int Dimension { get; private set; } = Core.Constants.FieldDimension;

        public ICellHandler GetCellHandler(int posX, int posY) => GetCellHandler(posX * Dimension + posY);

        public ICellHandler GetCellHandler(int cellIdx) => _cellHandlers.ElementAtOrDefault(cellIdx);

        #endregion

        #region IWpfFieldActivator

        public void Start(GameField newField, PlayerSide side, IStatusReporter reporter, IResultSender<IGameTurn> resultSetter)
        {
            _selectionController.IsSelectionAvaliable = true;
            _turnsController.UpdateState(newField, side, reporter, resultSetter);
        }

        public void Stop() => _selectionController.IsSelectionAvaliable = false;

        #endregion

        public void InitGameField(GameField gameField)
        {
            _cellHandlers.Clear();

            UpdateGameField(gameField, isCreateNew: true);

            RaiseReadrawField();
        }

        public void UpdateGameField(GameField gameField) => UpdateGameField(gameField, isCreateNew: false);

        private void RaiseReadrawField() => RedrawField?.Invoke(this, EventArgs.Empty);

        private void UpdateGameField(GameField gameField, bool isCreateNew)
        {
            _selectionController.Clear();

            for (var i = 0; i < Dimension; i++)
            {
                for (var j = 0; j < Dimension; j++)
                {
                    var cellIdx = i * Dimension + j;
                    var isCellActive = (i + j) % 2 != 0;

                    if (isCreateNew)
                    {
                        _cellHandlers.Add(new CellHandler(
                            cellIdx: cellIdx,
                            cellType: isCellActive ? CellColor.Black : CellColor.White,
                            cellState: gameField[cellIdx],
                            selectionController: _selectionController));
                    }
                    else
                    {
                        _cellHandlers[cellIdx].CellState = gameField[cellIdx];
                    }
                }
            }
        }
    }
}
