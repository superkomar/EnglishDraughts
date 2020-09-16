using System;
using System.Threading.Tasks;

using Core.Interfaces;

namespace Core.Helpers
{
    internal class SingleUseTaskProcessor<T> : ISingleUseResultProcessor<T>
    {
        private readonly TaskCompletionSource<T> _taskCompletionSource;

        public SingleUseTaskProcessor()
        {
            _taskCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        public void Cancel()
        {
            _taskCompletionSource.TrySetCanceled();
        }

        public async Task<T> WaitAsync()
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

        public void SetResult(T value)
        {
            _taskCompletionSource.TrySetResult(value);
        }
    }
}
