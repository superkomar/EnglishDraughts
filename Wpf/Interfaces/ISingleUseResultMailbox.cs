using System.Threading.Tasks;

namespace Wpf.Interfaces
{
    public interface IResultReceiver<T>
    {
        Task<T> ReceiveAsync();
    }

    public interface IResultSender<T>
    {
        void Send(T value);
    }

    public interface ISingleUseResultMailbox<T> : IResultReceiver<T>, IResultSender<T>
    {
        void Cancel();
    }
}
