using System.ComponentModel;

namespace Wpf.Interfaces
{
    public interface IEnableChanger<T> : INotifyPropertyChanged
    {
        T Property { get; set; }

        bool IsEnabled { get; set; }
    }
}