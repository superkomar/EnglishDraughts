using System.Windows.Input;

using EnglishDraughts.ViewModel.Enums;

namespace EnglishDraughts.ViewModel.Interfaces
{
    public interface IGameControllsVM
    {
        IEnableChanger<ICommand> StartGameCmd { get; }

        IEnableChanger<ICommand> RestartGameCmd { get; }

        IEnableChanger<PlayerSideType> PlayerSide { get; }

        string RobotTime { get; set; }

        RobotTypes RobotTypes { get; }
    }
}
