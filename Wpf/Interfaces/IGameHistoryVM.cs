using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using Wpf.ViewModels;

namespace Wpf.Interfaces
{
    public interface IGameHistoryVM : INotifyPropertyChanged
    {
        IList<HistoryItem> HistoryItems { get; }

        ICommand UndoCmd { get; }

        ICommand RedoCmd { get; }
    }
}
