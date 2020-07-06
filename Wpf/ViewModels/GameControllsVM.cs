using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;
using Wpf.ViewModels.Enums;

namespace Wpf.ViewModels
{
    public class RobotTypes
    {
        public string CurrentType { get; set; }

        public IList<string> AvailableTypes { get; }
    }

    public class GameControllsVM : NotifyPropertyChanged, IGameControllsVM
    {
        private readonly EnableChangerWrapper<PlayerSideType> _playerSide;

        public GameControllsVM()
        {
            StartGameCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(StartGameExecute), true);
            RestartGameCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(RestartGameExecute), true);

            _playerSide = new EnableChangerWrapper<PlayerSideType>(PlayerSideType.Black, true);
            _playerSide.PropertyChanged += OnPlayerSideChanged;
        }

        private void OnPlayerSideChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EnableChangerWrapper<PlayerSideType>.Control))
            {

            }
        }

        #region IGameControllsVM

        public IEnableChanger<PlayerSideType> PlayerSide => _playerSide;

        public IEnableChanger<ICommand> StartGameCmd { get; }

        public IEnableChanger<ICommand> RestartGameCmd { get; }

        public string RobotTime { get; set; }

        public RobotTypes RobotTypes => null;

        #endregion

        private void RestartGameExecute(object obj)
        {
            throw new NotImplementedException();
        }

        private void StartGameExecute(object obj)
        {
            PlayerSide.IsEnabled = false;
        }
    }
}
