namespace Core.Interfaces
{
    public interface IStatusReporter
    {
        string Info { get; }

        string Error { get; }

        void ReportInfo(string info);

        void ReportError(string error);
    }
}