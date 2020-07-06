using System.Collections.Generic;
using System.Windows.Input;

using Wpf.Interfaces;
using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    public class HistoryItem
    {
        public int Number { get; set; }

        public string FirstMove { get; set; }

        public string SecondMove { get; set; }
    }

    public class GameHistoryVM : NotifyPropertyChanged, IGameHistoryVM
    {
        public GameHistoryVM()
        {
            HistoryItems = new List<HistoryItem>
            {
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"},
                new HistoryItem { Number = 1, FirstMove = "1-2",   SecondMove = "25-24"},
                new HistoryItem { Number = 2, FirstMove = "13x16", SecondMove = "15x14"}
            };


        }

        public IList<HistoryItem> HistoryItems { get; }

        public ICommand UndoCmd { get; }

        public ICommand RedoCmd { get; }
    }
}
