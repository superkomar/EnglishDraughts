using System;
using System.IO;

using Core.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    public class StatusReporter : NotifyPropertyChanged, IReporter
    {
        private readonly TextWriter _errorWriter;
        private readonly TextWriter _infoWriter;

        public StatusReporter()
        {
            _errorWriter = Console.Error;
            _infoWriter = Console.Out;
        }

        public string Status { get; private set; }

        public void ReportError(string newError)
        {
            _errorWriter.WriteLine("!!! Error !!! " + newError);
        }

        public void ReportInfo(string newInfo)
        {
            _infoWriter.WriteLine("Info: " + newInfo);
        }

        public void ReportStatus(string newStatus)
        {
            if (Status == newStatus) return;

            Status = newStatus;
            OnPropertyChanged(nameof(Status));
        }
    }
}
