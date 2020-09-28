using System.ComponentModel;
using System.Threading.Tasks;

using Core;
using Core.Enums;
using Core.Interfaces;
using Core.Models;

using Robot;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    internal interface IMainWindowVM
    {
    }

    internal class MainWindowVM : ViewModelBase, IMainWindowVM
    {
        private GameController _gameController;

        private RobotPlayer _robot = new RobotPlayer();
        private string _statusText = "Wait start the game";

        public MainWindowVM()
        {
            Reporter = new StatusReporter();
            Reporter.ReportInfo(_statusText);

            AttachHandlers();
        }

        public IStatusReporter Reporter { get; }

        public string StatusText
        {
            get => _statusText;
            set => OnStatusTextChanged(value);
        }

        private void OnStatusTextChanged(string value)
        {
            if (value == _statusText)
            {
                return;
            }

            _statusText = value;
            OnPropertyChanged(nameof(StatusText));
        }

        private void AttachHandlers()
        {
            //VMLocator.GameFieldVM.PropertyChanged += OnFieldPropertyChanged;
            VMLocator.GameControllsVM.PropertyChanged += OnControllsPropertyChanged;
        }

        private void DetachHandlers()
        {
            //VMLocator.GameFieldVM.PropertyChanged -= OnFieldPropertyChanged;
            VMLocator.GameControllsVM.PropertyChanged -= OnControllsPropertyChanged;
        }

        private void OnControllsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IGameControllsVM.StartCmd):
                {
                    StartGameAsync();
                    break;
                }
                case nameof(IGameControllsVM.FinishCmd):
                {
                    _gameController?.StopGame();
                    break;
                }
                case nameof(IGameControllsVM.RobotTime):
                {
                    _robot.TurnTime = VMLocator.GameControllsVM.RobotTime;
                    break;
                }
                case nameof(IGameControllsVM.UndoCmd):
                {
                    _gameController?.Undo(deep: 2);
                    break;
                }
                case nameof(IGameControllsVM.RedoCmd):
                {
                    _gameController?.Redo(deep: 2);
                    break;
                }
            }
        }

        private async Task StartGameAsync()
        {
            var black = new WpfPlayer(VMLocator.GameFieldVM as IWpfTurnWaiter, Reporter);
            var white = new WpfPlayer(VMLocator.GameFieldVM as IWpfTurnWaiter, Reporter);

            _gameController = new GameController(Constants.FieldDimension, black, white);

            await foreach(var state in _gameController.StartGameAsync())
            {
                VMLocator.GameFieldVM.UpdateGameField(state.Field);

                switch (state.State)
                {
                    case GameState.StateType.Start:
                    {
                        black.StartGame(state.Field, PlayerSide.Black);
                        white.StartGame(state.Field, PlayerSide.White);
                        break;
                    }
                    case GameState.StateType.Finish:
                    {
                        black.FinishGame(state.Field, state.Side);
                        white.FinishGame(state.Field, state.Side);
                        return;
                    }
                    case GameState.StateType.Turn:
                        break;
                }
            }
        }
    }
}
