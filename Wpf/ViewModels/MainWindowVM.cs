using System.ComponentModel;
using System.Threading.Tasks;

using Core;
using Core.Interfaces;

using Robot;

using Wpf.CustomTypes;
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

        #region IStatusReporter

        public void Report(string playerStatus)
        {
            StatusText = playerStatus;
        }

        #endregion

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
                    // Undo
                    break;
                }
                case nameof(IGameControllsVM.RedoCmd):
                {
                    // Redo
                    break;
                }
            }
        }

        private async Task StartGameAsync()
        {
            _gameController = new GameController(Constants.FieldDimension,
                    LaunchStrategies.GetLauncher(VMLocator.GameFieldVM as IGamePlayer, LaunchStrategies.PlayerType.Human),
                    LaunchStrategies.GetLauncher(VMLocator.GameFieldVM as IGamePlayer, LaunchStrategies.PlayerType.Human),
                    Reporter);

            await foreach(var state in _gameController.StartGame())
            {
                VMLocator.GameFieldVM.UpdateGameField(state.Field);


                if (state.State == GameController.GameState.StateType.Turn &&
                    state.Side == Core.Enums.PlayerSide.)
            }
        }
    }
}
