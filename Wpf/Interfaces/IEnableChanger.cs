namespace Wpf.Interfaces
{
    public interface IEnableChanger<T>
    {
        T Control { get; set; }

        bool IsEnabled { get; set; }
    }
}