using System;
using System.Threading.Tasks;

using Wpf.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    public class SingleUseResultMailbox<T> : ISingleUseResultMailbox<T>
    {
        private readonly TaskCompletionSource<T> _taskCompletionSource;

        public SingleUseResultMailbox()
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
                result = await _taskCompletionSource.Task.ConfigureAwait(false);
            }
            catch (Exception)
            { }

            return result;
        }
    }
}
