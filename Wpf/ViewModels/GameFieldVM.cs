using System;
using System.Collections.Generic;
using System.Linq;

using Core.Enums;
using Core.Extensions;
using Core.Model;
using Core.Utils;

using Wpf.Interfaces;
using Wpf.Model;
using Wpf.ViewModels.CustomTypes;
using Wpf.ViewModels.Enums;

namespace Wpf.ViewModels
{
    public class GameFieldVM : ViewModelBase, IGameFieldVM, IPlayer
    {
        private readonly CustomObservableCollection<CellHandler> _cellHandlers;

        private GameField _currentField;
        private PlayerSide _playerSide;
        private CellHandler _selectedCells;

        private IEnumerable<GameTurn> _requiredJumps;

        public GameFieldVM()
        {
            _cellHandlers = new CustomObservableCollection<CellHandler>();

            Init(Dimension, PlayerSide.White);
        }

        #region IGameFieldVM

        public event EventHandler RedrawField;

        public int Dimension { get; private set; } = Constants.FieldDimension;

        public ICellHandler GetCellHandler(int posX, int posY) => GetCellHandler(posX * Dimension + posY);

        public ICellHandler GetCellHandler(int cellIdx) => _cellHandlers.ElementAtOrDefault(cellIdx);

        #endregion

        #region IPlayer

        public void Init(int dimension, PlayerSide side)
        {
            _playerSide = side;
            Dimension = dimension;

            _cellHandlers.Clear();

            UpdateGameField(ModelsCreator.CreateGameField(Dimension));

            RaiseRadrawFieldEvent();
        }

        public GameTurn MakeTurn(GameField gameField)
        {
            if (gameField.Dimension != Dimension) throw new ArgumentException("Faild!!! Incorrect game field");

            return null;
        }

        #endregion

        private void RaiseRadrawFieldEvent() => RedrawField?.Invoke(this, EventArgs.Empty);

        private bool TrySelectCell(CellHandler cellHandler)
        {
            if (cellHandler == null)
            {
                return false;
            }

            if (cellHandler.IsSelected)
            {
                _selectedCells = null;
                return true;
            }

            var state = cellHandler.CellState;

            if (_selectedCells == null && state != Core.Enums.CellState.Empty && state.ToPlayerSide() == _playerSide)
            {
                _selectedCells = cellHandler;
                return true;
            }

            return _selectedCells != null && _selectedCells != cellHandler && TryMakeTurn(cellHandler);
        }

        private bool TryMakeTurn(CellHandler cellHandler)
        {
            var gameTurn = ModelsCreator.CreateGameTurn(_currentField, _playerSide, _selectedCells.CellIdx, cellHandler.CellIdx);

            if (gameTurn != null 
                && (!gameTurn.IsSimple || !_requiredJumps.Any())
                && GameFieldUpdater.TryMakeTurn(_currentField, gameTurn, out GameField newField))
            {
                UpdateGameField(newField);

                _selectedCells.IsSelected = false;

                if (_requiredJumps.Any(x => x.Turns.First() == cellHandler.CellIdx))
                {
                    _selectedCells = cellHandler;
                    return true;
                }
            }

            return false;
        }

        private void UpdateGameField(GameField gameField)
        {
            _currentField = gameField;

            void updateAction(int idx, CellHandler handler)
            {
                if ((_cellHandlers.Count - 1) > idx) _cellHandlers[idx].CellState = handler.CellState;
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
                        isCellActive ? CellType.Black : CellType.White,
                        gameField[cellIdx],
                        TrySelectCell);

                    updateAction(cellIdx, cellHandler);
                }
            }

            _requiredJumps = GameFieldUtils.FindRequiredJumps(_currentField, _playerSide);
        }
    }
}
