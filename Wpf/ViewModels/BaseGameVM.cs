using Wpf.ViewModels.CustomTypes;

namespace Wpf.ViewModels
{
    public abstract class BaseGameVM : NotifyPropertyChanged
    {
        public bool IsRun { get; set; }

        public abstract void ChangeStateByTurn();
    }
}
