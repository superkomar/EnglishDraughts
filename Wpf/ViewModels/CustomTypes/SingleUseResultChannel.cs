using System;
using System.Threading.Tasks;

using Wpf.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    public class SingleUseResultChannel<T> : ISingleUseResultChannel<T>
    {
        private readonly TaskCompletionSource<T> _taskCompletionSource;

        public SingleUseResultChannel()
        {
            _taskCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        public void Cancel() => _taskCompletionSource.TrySetCanceled();

        public void Send(T value) => _taskCompletionSource.TrySetResult(value);

        public async Task<T> ReceiveAsync()
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
    }
}
