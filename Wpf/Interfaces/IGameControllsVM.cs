using System.ComponentModel;
using System.Windows.Input;

using Wpf.ViewModels;
using Wpf.ViewModels.Enums;

namespace Wpf.Interfaces
{
    public interface IGameControllsVM : INotifyPropertyChanged
    {
        IEnableChanger<ICommand> StartGameCmd { get; }

        IEnableChanger<ICommand> RestartGameCmd { get; }

        IEnableChanger<PlayerSideType> PlayerSide { get; }

        string RobotTime { get; set; }

        RobotTypes RobotTypes { get; }
    }
}
