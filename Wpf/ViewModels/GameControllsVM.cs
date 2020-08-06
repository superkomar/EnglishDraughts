using System.Windows.Input;

using Core.Enums;

//using Wpf.Enums;
using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    public class GameControllsVM : ViewModelBase, IGameControllsVM
    {
        private int _robotTimeMs = (int) 1E5;

        public GameControllsVM()
        {
            StartCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(StartCmdExecute), isEnable: true);
            FinishCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(FinishCmdExecute), isEnable: false);
            UndoCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(UndoCmdExecute), isEnable: false);
            RedoCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(RedoCmdExecute), isEnable: false);
            Side = new EnableChangerWrapper<PlayerSide>(PlayerSide.White, isEnable: true);
        }

        #region IGameControllsVM

        public int RobotTime
        {
            get => _robotTimeMs;
            set => OnRobotTomeChanged(value);
        }

        public IEnableChanger<PlayerSide> Side { get; set; }

        public IEnableChanger<ICommand> FinishCmd { get; }

        public IEnableChanger<ICommand> StartCmd { get; }

        public IEnableChanger<ICommand> UndoCmd { get; }

        public IEnableChanger<ICommand> RedoCmd { get; }

        #endregion

        private void UpdateControlStates(bool isGameStarted)
        {
            Side.IsEnabled = !isGameStarted;
            StartCmd.IsEnabled = !isGameStarted;

            UndoCmd.IsEnabled = isGameStarted;
            RedoCmd.IsEnabled = isGameStarted;
            FinishCmd.IsEnabled = isGameStarted;
        }

        private void OnRobotTomeChanged(int value)
        {
            if (value == RobotTime || value <= 0) return;

            _robotTimeMs = value;
            OnPropertyChanged(nameof(RobotTime));
        }

        private void FinishCmdExecute(object obj)
        {
            UpdateControlStates(isGameStarted: false);

            OnPropertyChanged(nameof(FinishCmd));
        }
        
        private void StartCmdExecute(object obj)
        {
            UpdateControlStates(isGameStarted: true);

            OnPropertyChanged(nameof(StartCmd));
        }

        private void RedoCmdExecute(object obj) => OnPropertyChanged(nameof(RedoCmd));

        private void UndoCmdExecute(object obj) => OnPropertyChanged(nameof(UndoCmd));
    }
}
