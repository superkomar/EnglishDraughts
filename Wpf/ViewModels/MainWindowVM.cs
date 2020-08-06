using System;
using System.ComponentModel;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Model;

using Wpf.Interfaces;
using Wpf.Model;

namespace Wpf.ViewModels
{
    internal interface IMainWindowVM : INotifyPropertyChanged
    {
    }

    public class RobotPlayer : IGamePlayer
    {
        public int TurnTime { get; set; }

        public void EndGame(PlayerSide winner)
        {
            throw new NotImplementedException();
        }

        public void StartGame(int dimension, PlayerSide side, IStatusReporter statusReporter)
        {
            throw new NotImplementedException();
        }

        public async Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side)
        {
            throw new NotImplementedException();
        }
    }

    internal class MainWindowVM : ViewModelBase, IMainWindowVM, IStatusReporter
    {
        private PlayersController _gameController;

        private RobotPlayer _robot = new RobotPlayer();
        private string _statusText = "Wait start the game";

        public MainWindowVM()
        {
            

            AttachHandlers();
        }

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

        private async void OnControllsPropertyChanged(object sender, PropertyChangedEventArgs e)
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
            _gameController = new PlayersController(
                    Constants.FieldDimension,
                    VMLocator.GameFieldVM as IGamePlayer,
                    VMLocator.GameFieldVM as IGamePlayer,
                    this);

            _gameController.StartGame();
        }
    }
}
