using Wpf.Interfaces;

namespace Wpf.ViewModels
{
    internal static class VMLocator
    {
        public static ICellHandlersController CellHandlersController => GameFieldVM;

        public static IGameControlsVM IGameControlsVM => GameControlsVM;

        public static GameControlsVM GameControlsVM { get; } = new GameControlsVM();

        public static GameFieldVM GameFieldVM { get; } = new GameFieldVM();
    }
}
