using System.Windows.Input;
using Wpf.Enums;

//using Core.Enums;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    public class GameControllsVM : ViewModelBase, IGameControllsVM
    {
        private int _robotTimeMs = (int) 1E5;

        public GameControllsVM()
        {
            StartCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(StartCmdExecute), true);
            FinishCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(FinishCmdExecute), true);
            UndoCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(UndoCmdExecute), true);
            RedoCmd = new EnableChangerWrapper<ICommand>(new RelayCommand(RedoCmdExecute), true);
            Side = new EnableChangerWrapper<PlayerSide>(PlayerSide.White, true);
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

        private void OnRobotTomeChanged(int value)
        {
            if (value == RobotTime || value <= 0) return;

            _robotTimeMs = value;
            OnPropertyChanged(nameof(RobotTime));
        }

        private void FinishCmdExecute(object obj)
        {
            Side.IsEnabled = true;
            UndoCmd.IsEnabled = false;
            RedoCmd.IsEnabled = false;
            StartCmd.IsEnabled = false;
            FinishCmd.IsEnabled = true;

            OnPropertyChanged(nameof(FinishCmd));
        }
        
        private void StartCmdExecute(object obj)
        {
            Side.IsEnabled = false;
            UndoCmd.IsEnabled = true;
            RedoCmd.IsEnabled = true;
            StartCmd.IsEnabled = false;
            FinishCmd.IsEnabled = true;

            OnPropertyChanged(nameof(StartCmd));
        }

        private void RedoCmdExecute(object obj)
        {
            OnPropertyChanged(nameof(RedoCmd));
        }

        private void UndoCmdExecute(object obj)
        {
            OnPropertyChanged(nameof(UndoCmd));
        }
    }
}
