using System.ComponentModel;
using System.Windows.Input;

using Core.Enums;

namespace Wpf.Interfaces
{
    internal interface IGameControllsVM : INotifyPropertyChanged
    { 
        IEnableChanger<ICommand> FinishCmd { get; }
        
        IEnableChanger<ICommand> StartCmd { get; }
        
        IEnableChanger<ICommand> UndoCmd { get; }
        
        IEnableChanger<ICommand> RedoCmd { get; }

        IEnableChanger<PlayerSide> Side { get; set; }
        
        int RobotTime { get; set; }
    }
}
