using System;
using System.Collections.Generic;
using System.Linq;

using Core;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Core.Utils;

using Wpf.Interfaces;
using Wpf.Properties;
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

            InitGameField(FieldUtils.CreateField(Dimension));
        }

        #region ICellHandlersController

        public event EventHandler RedrawField;

        public int Dimension { get; private set; } = Settings.Default.DefaultFieldDimension;

        public ICellHandler GetCellHandler(int posX, int posY) => GetCellHandler(posX * Dimension + posY);

        public ICellHandler GetCellHandler(int cellIdx) => _cellHandlers.ElementAtOrDefault(cellIdx);

        #endregion

        #region IWpfFieldActivator

        public void Start(GameField newField, PlayerSide side, IReporter reporter, IResultSender<IGameTurn> resultSetter)
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

            RiseRedrawFieldEvent();
        }

        public void UpdateGameField(GameField gameField) => UpdateGameField(gameField, isCreateNew: false);

        private void RiseRedrawFieldEvent() => RedrawField?.Invoke(this, EventArgs.Empty);

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
