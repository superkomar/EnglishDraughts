using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IResultWaiter<T>
    {
        Task<T> WaitAsync();
    }

    public interface IResultSetter<T>
    {
        void SetResult(T value);
    }

    public interface ISingleUseResultProcessor<T> : IResultWaiter<T>, IResultSetter<T>
    { }
}
