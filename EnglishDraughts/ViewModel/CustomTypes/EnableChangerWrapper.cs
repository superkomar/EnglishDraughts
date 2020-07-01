using System;
using System.Collections.Generic;
using EnglishDraughts.ViewModel.Interfaces;

namespace EnglishDraughts.ViewModel.CustomTypes
{
    public class EnableChangerWrapper<T> : NotifyPropertyChanged, IEnableChanger<T>
    {
        private T _control;
        private bool _isEnabled;

        public EnableChangerWrapper(T property, bool isEnable)
        {
            Control = property;
            IsEnabled = isEnable;
        }

        public T Control
        {
            get => _control;
            set => OnControlChanged(value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => OnIsEnabledChanged(value);
        }

        private void OnControlChanged(T value)
        {
            if (EqualityComparer<T>.Default.Equals(_control, value)) return;

            _control = value;
            OnPropertyChanged(nameof(Control));
        }

        private void OnIsEnabledChanged(bool value)
        {
            if (_isEnabled == value) return;

            _isEnabled = value;
            OnPropertyChanged(nameof(IsEnabled));
        }
    }
}
