using System.Collections.Generic;
using System.Windows.Input;

namespace EnglishDraughts.ViewModel.Interfaces
{
    public interface IGameHistoryVM
    {
        IList<HistoryItem> HistoryItems { get; }

        ICommand UndoCmd { get; }

        ICommand RedoCmd { get; }
    }
}
