
using Core.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    public class StatusReporter : NotifyPropertyChanged, IStatusReporter
    {
        private string _status;

        public StatusReporter(string startStatus = "")
        {
            Status = startStatus;
        }
        
        public string Status
        {
            get => _status;
            set => OnStatusChanged(value);
        }

        private void OnStatusChanged(string newStatus)
        {
            if (Status == newStatus) return;

            _status = newStatus;
            OnPropertyChanged(nameof(Status));
        }
    }
}
