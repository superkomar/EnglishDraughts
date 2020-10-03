using System.Collections.Generic;

namespace Wpf.ViewModels.CustomTypes
{
    public class ValueWithEnableToggle<T> : NotifyPropertyChanged
    {
        private bool _isEnabled;
        private T _property;
        
        public ValueWithEnableToggle(T property, bool isEnable)
        {
            Value = property;
            IsEnabled = isEnable;
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => OnIsEnabledChanged(value);
        }

        public T Value
        {
            get => _property;
            set => OnPropertyChanged(value);
        }
        
        private void OnIsEnabledChanged(bool value)
        {
            if (_isEnabled == value) return;

            _isEnabled = value;
            OnPropertyChanged(nameof(IsEnabled));
        }

        private void OnPropertyChanged(T value)
        {
            if (EqualityComparer<T>.Default.Equals(_property, value)) return;

            _property = value;
            OnPropertyChanged(nameof(Value));
        }
    }
}
