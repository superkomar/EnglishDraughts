using System.ComponentModel;

namespace Core.Interfaces
{
    public interface IStatusReporter : INotifyPropertyChanged
    {
        string Status { get; set; }
    }
}