using System;

using Core.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    public class StatusReporter : NotifyPropertyChanged, IReporter
    {
        public string Error { get; private set; }

        public string Status { get; private set; }

        public string Info { get; private set; }

        public void ReportError(string error)
        {
            Error = error;
            OnPropertyChanged(nameof(Error));

            Console.Error.WriteLine("!!! Error !!! " + error);
        }

        public void ReportInfo(string info)
        {
            Info = info;
            OnPropertyChanged(nameof(Info));

            Console.Out.WriteLine("Info: " + info);
        }

        public void ReportStatus(string status)
        {
            Status = status;
            OnPropertyChanged(nameof(Status));
        }
    }
}
