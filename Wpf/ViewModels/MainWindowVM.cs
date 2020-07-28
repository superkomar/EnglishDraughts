using System;
using System.ComponentModel;

using Wpf.Interfaces;
using Wpf.Model;

namespace Wpf.ViewModels
{
    internal interface IMainWindowVM : INotifyPropertyChanged
    {
    }

    internal class MainWindowVM : ViewModelBase, IMainWindowVM
    {
        public readonly PlayersController _gameController;

        public MainWindowVM()
        {
            _gameController = new PlayersController();

            AttachHandlers();
        }

        public string StatusText => "Wait start the game";

        private void AttachHandlers()
        {
            VMLocator.GameFieldVM.PropertyChanged += OnFieldPropertyChanged;
            VMLocator.GameHistoryVM.PropertyChanged += OnHistoryPropertyChanged;
            VMLocator.GameControllsVM.PropertyChanged += OnControllsPropertyChanged;
        }

        private void OnControllsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == nameof(IGameControllsVM.StartGameCmd))
            //{
            //    //_gameController.StartGame();
            //}
            //else if (e.PropertyName == nameof(IGameControllsVM.RestartGameCmd))
            //{

            //}
            //else if (e.PropertyName == nameof(IGameControllsVM.RobotTime))
            //{

            //}
        }

        private void OnFieldPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //_gameController.
        }
        
        private void OnHistoryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IGameHistoryVM.RedoCmd))
            {

            }
            else if (e.PropertyName == nameof(IGameHistoryVM.UndoCmd))
            {

            }
        }
    }
}
