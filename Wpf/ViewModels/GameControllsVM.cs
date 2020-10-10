using System.Windows.Input;

using Core.Enums;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    internal class GameControllsVM : NotifyPropertyChanged, IGameControllsVM, IWpfControlsActivator
    {
        public enum StateType
        {
            GameStart,
            GameFinish,
            WpfTurnStart,
            WpfTurnStop
        }

        public GameControllsVM()
        {
            StartCmd = new ValueWithEnableToggle<ICommand>(new RelayCommand(StartCmdExecute), isEnable: true);
            FinishCmd = new ValueWithEnableToggle<ICommand>(new RelayCommand(FinishCmdExecute), isEnable: false);

            UndoCmd = new ValueWithEnableToggle<ICommand>(new RelayCommand(UndoCmdExecute), isEnable: false);
            RedoCmd = new ValueWithEnableToggle<ICommand>(new RelayCommand(RedoCmdExecute), isEnable: false);

            Side = new ValueWithEnableToggle<PlayerSide>(PlayerSide.White, isEnable: true);
            RobotTime = new ValueWithEnableToggle<int>(Constants.DefaultRobotTimeMs, isEnable: false, nameof(RobotTime));
        }

        #region IGameControllsVM

        public ValueWithEnableToggle<int> RobotTime { get; set; }

        public ValueWithEnableToggle<PlayerSide> Side { get; set; }
        
        public ValueWithEnableToggle<ICommand> StartCmd { get; }

        public ValueWithEnableToggle<ICommand> FinishCmd { get; }

        public ValueWithEnableToggle<ICommand> UndoCmd { get; }

        public ValueWithEnableToggle<ICommand> RedoCmd { get; }

        #endregion

        #region IWpfControlsActivator

        public void StartTurn() => UpdateState(StateType.WpfTurnStart);

        public void StopTurn() => UpdateState(StateType.WpfTurnStop);

        #endregion
        
        public void UpdateState(StateType state)
        {
            switch (state)
            {
                case StateType.GameStart: UpdateControlStates(isGameStarted: true); break;
                case StateType.GameFinish: UpdateControlStates(isGameStarted: false); break;
                case StateType.WpfTurnStart: WpfTurnStart(isTurnStarting: true); break;
                case StateType.WpfTurnStop: WpfTurnStart(isTurnStarting: false); break;
            }
        }

        private void FinishCmdExecute(object obj)
        {
            UpdateState(StateType.GameFinish);

            OnPropertyChanged(nameof(FinishCmd));
        }

        private void RedoCmdExecute(object obj) => OnPropertyChanged(nameof(RedoCmd));

        private void StartCmdExecute(object obj)
        {
            UpdateState(StateType.GameStart);

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

        private void WpfTurnStart(bool isTurnStarting)
        {
            UndoCmd.IsEnabled = RedoCmd.IsEnabled = RobotTime.IsEnabled = isTurnStarting;
        }
    }
}
