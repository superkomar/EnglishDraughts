using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

using Core;
using Core.Enums;
using Core.Interfaces;
using Core.Model;
using Core.Utils;

using Wpf.Interfaces;
using Wpf.Model;
using Wpf.ViewModels.CustomTypes;
using Wpf.ViewModels.Enums;

namespace Wpf.ViewModels
{
    public class GameFieldVM : ViewModelBase, IGameFieldVM, IGamePlayer
    {
        private readonly CustomObservableCollection<CellHandler> _cellHandlers; // ???
        private readonly SelectionController _selectionController;
        private readonly TurnsController _turnsController;

        private bool _isActive;
        private PlayerSide _playerSide;
        private IStatusReporter _statusReporter;
        private GameField _currField;

        public GameFieldVM()
        {
            _cellHandlers = new CustomObservableCollection<CellHandler>();

            _turnsController = new TurnsController();
            _selectionController = new SelectionController(_turnsController);

            InitGame(Dimension, PlayerSide.White, null);
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

        #region IPlayer

        public void InitGame(int dimension, PlayerSide side, IStatusReporter reporter)
        {
            _playerSide = side;
            Dimension = dimension;
            _statusReporter = reporter;

            _cellHandlers.Clear();

            _currField = ModelsCreator.CreateGameField(Dimension);

            UpdateGameField(_currField);
            _turnsController.UpdateField(_currField, side, reporter, null);

            OnRadrawFieldChanged();
        }

        public void FinishGame(PlayerSide winner)
        {

        }

        private TaskCompletionSource<IGameTurn> _turnCompletionTask;

        public async Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side)
        {
            if (gameField.Dimension != Dimension) throw new ArgumentException("Failed!!! Incorrect game field");

            IsActive = true;

            var turn = await GetTurnTask(gameField, side);

            IsActive = false;

            return turn;
        }

        #endregion

        private async Task<IGameTurn> GetTurnTask(GameField gameField, PlayerSide side)
        {
            // First methond
            {
                _turnCompletionTask = new TaskCompletionSource<IGameTurn>();

                IGameTurn turn = null;

                try
                {
                    var tmp = Dispatcher.CurrentDispatcher.BeginInvoke(() => UpdateGameField(gameField, true), DispatcherPriority.Render);
                    //UpdateGameField(gameField, true);
                    //await Task.Run(() => UpdateGameField(gameField)).ConfigureAwait(false);
                    //Dispatcher.Invoke(() => UpdateGameField(gameField));

                    _turnsController.UpdateField(gameField, side, _statusReporter, _turnCompletionTask);

                    turn = await _turnCompletionTask.Task;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Some shit with TaskCompletionSource!");
                }

                return turn;
            }

            // Second method
            {
                //var turnCompletionTask = new TaskCompletionSource<IGameTurn>();

                //UpdateGameField(gameField, true);

                //void TakeTurnHandler(object o, IGameTurn args)
                //{
                //    try
                //    {
                //        //_selectionController.ClearSelection();
                //        turnCompletionTask.SetResult(args);
                //    }
                //    catch (Exception ex)
                //    {
                //        turnCompletionTask.SetException(ex);
                //    }
                //}

                //_turnsController.TakeTurn += TakeTurnHandler;

                //try
                //{
                //    return await turnCompletionTask.Task;
                //}
                //finally
                //{
                //    _turnsController.TakeTurn -= TakeTurnHandler;
                //}
            }
        }

        private void OnIsActiveChanged(bool value)
        {
            if (value == IsActive) return;

            _isActive = value;
            OnPropertyChanged(nameof(IsActive));
        }
        
        private void OnRadrawFieldChanged() => RedrawField?.Invoke(this, EventArgs.Empty);

        private void UpdateGameField(GameField gameField, bool flag = false)
        {
            var cellIdx = 0;
            try
            {
                _currField = gameField;

                void updateAction(int idx, CellHandler handler)
                {
                    if ((_cellHandlers.Count - 1) > idx) _cellHandlers[idx].CellState = handler.CellState;
                    else _cellHandlers.Add(handler);
                }

                for (var i = 0; i < Dimension; i++)
                {
                    for (var j = 0; j < Dimension; j++)
                    {
                        cellIdx = i * Dimension + j;
                        var isCellActive = (i + j) % 2 != 0;

                        var cellHandler = new CellHandler(
                            cellIdx,
                            isCellActive ? CellType.Black : CellType.White,
                            gameField[cellIdx],
                            _selectionController);

                        if (flag && cellIdx == 12)
                        {
                            cellHandler = new CellHandler(
                                cellIdx,
                                isCellActive ? CellType.Black : CellType.White,
                                CellState.Empty,
                                _selectionController);
                        }

                        updateAction(cellIdx, cellHandler);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
