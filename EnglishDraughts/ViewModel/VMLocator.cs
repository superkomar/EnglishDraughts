using System;

using EnglishDraughts.ViewModel.Interfaces;

namespace EnglishDraughts.ViewModel
{
    public static class VMLocator
    {
        private static readonly Lazy<IGameHistoryVM> _lazyGameHistory = new Lazy<IGameHistoryVM>(new GameHistoryVM());
        private static readonly Lazy<IGameControllsVM> _lazyGameControlls = new Lazy<IGameControllsVM>(new GameControllsVM());
        private static readonly Lazy<IGameFieldVM> _lazyGameFieldVM = new Lazy<IGameFieldVM>(new GameFieldVM());

        public static IGameHistoryVM GameHistoryVM => _lazyGameHistory?.Value;

        public static IGameFieldVM GameFieldVM => _lazyGameFieldVM?.Value;

        public static IGameControllsVM GameControllsVM => _lazyGameControlls?.Value;
    }
}
