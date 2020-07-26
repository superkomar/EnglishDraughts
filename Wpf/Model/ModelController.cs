using System;

using Core.Model;

using Wpf.ViewModels.CustomTypes;

namespace Wpf.Model
{
    public class ModelController : NotifyPropertyChanged
    {
        public static ModelController Model { get; } = new ModelController();

        public event EventHandler FieldUpdated;

        public GameField CurrentGameField { get; private set; }
    }
}
