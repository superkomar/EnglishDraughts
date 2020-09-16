using Wpf.Interfaces;

namespace Wpf.ViewModels
{
    internal static class VMLocator
    {
        public static IGameFieldVM GameFieldVM { get; } = new GameFieldVM();

        public static IGameControllsVM GameControllsVM { get; } = new GameControllsVM();
    }
}
