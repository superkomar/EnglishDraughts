using Wpf.Interfaces;

namespace Wpf.ViewModels
{
    public static class VMLocator
    {
        public static IGameHistoryVM GameHistoryVM { get; } = new GameHistoryVM();

        public static IGameFieldVM GameFieldVM { get; } = new GameFieldVM();

        public static IGameControllsVM GameControllsVM { get; } = new GameControllsVM();
    }
}
