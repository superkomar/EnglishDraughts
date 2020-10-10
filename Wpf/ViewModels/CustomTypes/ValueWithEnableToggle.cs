using System.Collections.Generic;

namespace Wpf.ViewModels.CustomTypes
{
    public class ValueWithEnableToggle<T> : NotifyPropertyChanged
    {
        private readonly string _additionalName;

        private bool _isEnabled;
        private T _property;
        
        public ValueWithEnableToggle(T property, bool isEnable, string additionalName = "")
        {
            Value = property;
            IsEnabled = isEnable;

            _additionalName = additionalName;
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
            OnPropertyChanged(_additionalName);
        }
    }
}
