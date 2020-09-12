using Core.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    public class StatusReporter : NotifyPropertyChanged, IStatusReporter
    {
        public string Error { get; private set; }

        public string Info { get; private set; }

        public void ReportError(string error)
        {
            if (Error == error)
            {
                return;
            }

            Error = error;
            OnPropertyChanged(nameof(Error));
        }

        public void ReportInfo(string info)
        {
            if (Info == info)
            {
                return;
            }

            Info = info;
            OnPropertyChanged(nameof(Info));
        }
    }
}
