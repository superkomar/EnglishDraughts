using System.ComponentModel;
using System.Windows.Input;

using Core.Enums;

using Wpf.ViewModels.CustomTypes;

namespace Wpf.Interfaces
{
    internal interface IGameControlsVM : INotifyPropertyChanged
    {
        ValueWithEnableToggle<ICommand> EndGameCmd { get; }

        ValueWithEnableToggle<ICommand> StartGameCmd { get; }

        ValueWithEnableToggle<ICommand> UndoCmd { get; }

        ValueWithEnableToggle<ICommand> RedoCmd { get; }

        ValueWithEnableToggle<PlayerSide> Side { get; set; }
        
        ValueWithEnableToggle<int> RobotTime { get; set; }
    }
}
