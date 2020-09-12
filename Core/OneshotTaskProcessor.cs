using System;
using System.Threading.Tasks;

namespace Core
{
    public interface ITaskGetter<T>
    {
        Task<T> Get();
    }

    public interface ITaskSetter<T>
    {
        void Set(T value);
    }

    public interface IOneshotTaskProcessor<T> : ITaskGetter<T>, ITaskSetter<T>
    { }

    public class OneshotTaskProcessor<T> : IOneshotTaskProcessor<T>
    {
        private readonly TaskCompletionSource<T> _taskCompletionSource;

        public OneshotTaskProcessor()
        {
            _taskCompletionSource = new TaskCompletionSource<T>();
        }

        public void Set(T value)
        {
            _taskCompletionSource.TrySetResult(value);
        }

        public async Task<T> Get()
        {
            T result = default;

            try
            {
                result = await _taskCompletionSource.Task;
            }
            catch (Exception)
            { }

            return result;
        }

        public void Cancel()
        {
            _taskCompletionSource.TrySetCanceled();
        }
    }
}
