using System.ComponentModel;
using System.Windows.Input;

using Core.Enums;

using Wpf.ViewModels.CustomTypes;

namespace Wpf.Interfaces
{
    internal interface IGameControllsVM : INotifyPropertyChanged
    {
        ValueWithEnableToggle<ICommand> FinishCmd { get; }

        ValueWithEnableToggle<ICommand> StartCmd { get; }

        ValueWithEnableToggle<ICommand> UndoCmd { get; }

        ValueWithEnableToggle<ICommand> RedoCmd { get; }

        ValueWithEnableToggle<PlayerSide> Side { get; set; }
        
        ValueWithEnableToggle<int> RobotTime { get; set; }
    }
}
