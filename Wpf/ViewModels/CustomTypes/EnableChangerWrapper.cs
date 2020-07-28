using System.Collections.Generic;

using Wpf.Interfaces;

namespace Wpf.ViewModels.CustomTypes
{
    public class EnableChangerWrapper<T> : NotifyPropertyChanged, IEnableChanger<T>
    {
        private T _property;
        private bool _isEnabled;

        public EnableChangerWrapper(T property, bool isEnable)
        {
            Property = property;
            IsEnabled = isEnable;
        }

        public T Property
        {
            get => _property;
            set => OnPropertyChanged(value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => OnIsEnabledChanged(value);
        }

        private void OnPropertyChanged(T value)
        {
            if (EqualityComparer<T>.Default.Equals(_property, value)) return;

            _property = value;
            OnPropertyChanged(nameof(Property));
        }

        private void OnIsEnabledChanged(bool value)
        {
            if (_isEnabled == value) return;

            _isEnabled = value;
            OnPropertyChanged(nameof(IsEnabled));
        }
    }
}
