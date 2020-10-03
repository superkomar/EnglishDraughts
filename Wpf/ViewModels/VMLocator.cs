using Wpf.Interfaces;

namespace Wpf.ViewModels
{
    internal static class VMLocator
    {
        public static ICellHandlersController CellHandlersController => GameFieldVM;

        public static IGameControllsVM IGameControllsVM => GameControllsVM;

        public static GameControllsVM GameControllsVM { get; } = new GameControllsVM();

        public static GameFieldVM GameFieldVM { get; } = new GameFieldVM();
    }
}
