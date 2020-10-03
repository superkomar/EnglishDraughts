namespace Core.Interfaces
{
    public interface IStatusReporter
    {
        string Error { get; }
        
        string Info { get; }
        
        void ReportError(string error);

        void ReportInfo(string info);
    }
}