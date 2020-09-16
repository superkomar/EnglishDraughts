using System;
using System.Linq;

using Core.Enums;
using Core.Interfaces;
using Core.Model;

using Wpf.CustomTypes;
using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;
using Wpf.ViewModels.Enums;

namespace Wpf.ViewModels
{
    interface IInterface1
    {
        bool IsActive { get; set; }

        void Update(GameField newField, PlayerSide side, IStatusReporter reporter, ITaskSetter<IGameTurn> sender);
    }

    internal class GameFieldVM : ViewModelBase, IGameFieldVM, IInterface1
    {
        private readonly CustomObservableCollection<CellHandler> _cellHandlers; // ???
        private readonly SelectionController _selectionController;
        private readonly TurnsConstructor _turnsController;

        private bool _isActive;

        public GameFieldVM()
        {
            _cellHandlers = new CustomObservableCollection<CellHandler>();

            _turnsController = new TurnsConstructor();
            _selectionController = new SelectionController(_turnsController);

            Parameters = new PlayerParameters();
        }

        private class PlayerParameters : IPlayerParameters
        {
            public int TurnTime => -1;

            public LaunchStrategies.PlayerType Type => LaunchStrategies.PlayerType.Human;
        }

        #region IGameFieldVM

        public event EventHandler RedrawField;

        public int Dimension { get; private set; } = Constants.FieldDimension;

        public bool IsActive
        {
            get => _isActive;
            set => OnIsActiveChanged(value);
        }

        public IPlayerParameters Parameters { get; private set; }

        public ICellHandler GetCellHandler(int posX, int posY) => GetCellHandler(posX * Dimension + posY);

        public ICellHandler GetCellHandler(int cellIdx) => _cellHandlers.ElementAtOrDefault(cellIdx);

        #endregion

        #region IInterface1

        public void Update(GameField newField, PlayerSide side, IStatusReporter reporter, ITaskSetter<IGameTurn> resultSetter) =>
            _turnsController.Restart(newField, side, reporter, resultSetter);

        #endregion

        private void OnIsActiveChanged(bool value)
        {
            if (value == IsActive) return;

            _isActive = value;
            OnPropertyChanged(nameof(IsActive));
        }

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
