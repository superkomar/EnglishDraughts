using System.ComponentModel;
using System.Windows.Input;
using Wpf.Enums;

//using Core.Enums;

namespace Wpf.Interfaces
{
    public interface IGameControllsVM : INotifyPropertyChanged
    { 
        IEnableChanger<ICommand> FinishCmd { get; }
        
        IEnableChanger<ICommand> StartCmd { get; }
        
        IEnableChanger<ICommand> UndoCmd { get; }
        
        IEnableChanger<ICommand> RedoCmd { get; }

        IEnableChanger<PlayerSide> Side { get; set; }
        
        int RobotTime { get; set; }
    }
}
