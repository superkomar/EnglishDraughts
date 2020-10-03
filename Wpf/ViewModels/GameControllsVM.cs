using System.Windows.Input;

using Core.Enums;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    internal class GameControllsVM : NotifyPropertyChanged, IGameControllsVM, IWpfControlsActivator
    {
        public GameControllsVM()
        {
            StartCmd = new ValueWithEnableToggle<ICommand>(new RelayCommand(StartCmdExecute), isEnable: true);
            FinishCmd = new ValueWithEnableToggle<ICommand>(new RelayCommand(FinishCmdExecute), isEnable: false);

            UndoCmd = new ValueWithEnableToggle<ICommand>(new RelayCommand(UndoCmdExecute), isEnable: false);
            RedoCmd = new ValueWithEnableToggle<ICommand>(new RelayCommand(RedoCmdExecute), isEnable: false);
            
            Side = new ValueWithEnableToggle<PlayerSide>(PlayerSide.White, isEnable: true);
            RobotTime = new ValueWithEnableToggle<int>(Constants.DefaultRobotTimeMs, isEnable: false);
        }

        #region IGameControllsVM

        public ValueWithEnableToggle<ICommand> FinishCmd { get; }
        
        public ValueWithEnableToggle<ICommand> RedoCmd { get; }
        
        public ValueWithEnableToggle<int> RobotTime { get; set; }

        public ValueWithEnableToggle<PlayerSide> Side { get; set; }
        
        public ValueWithEnableToggle<ICommand> StartCmd { get; }

        public ValueWithEnableToggle<ICommand> UndoCmd { get; }
        
        #endregion

        #region IWpfControlsActivator

        public void StartTurn() =>
            UndoCmd.IsEnabled = RedoCmd.IsEnabled = RobotTime.IsEnabled = true;

        public void StopTurn() =>
            UndoCmd.IsEnabled = RedoCmd.IsEnabled = RobotTime.IsEnabled = false;

        #endregion

        private void FinishCmdExecute(object obj)
        {
            UpdateControlStates(isGameStarted: false);

            OnPropertyChanged(nameof(FinishCmd));
        }

        private void RedoCmdExecute(object obj) => OnPropertyChanged(nameof(RedoCmd));

        private void StartCmdExecute(object obj)
        {
            UpdateControlStates(isGameStarted: true);

            OnPropertyChanged(nameof(StartCmd));
        }

        private void UndoCmdExecute(object obj) => OnPropertyChanged(nameof(UndoCmd));

        private void UpdateControlStates(bool isGameStarted)
        {
            Side.IsEnabled = !isGameStarted;
            StartCmd.IsEnabled = !isGameStarted;

            UndoCmd.IsEnabled = isGameStarted;
            RedoCmd.IsEnabled = isGameStarted;
            FinishCmd.IsEnabled = isGameStarted;
        }
    }
}
