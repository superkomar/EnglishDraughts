namespace Core.Interfaces
{
    public interface IReporter
    {
        string Status { get; }

        void ReportError(string error);

        void ReportInfo(string error);

        void ReportStatus(string info);
    }
}