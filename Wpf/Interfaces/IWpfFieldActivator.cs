using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Wpf.Interfaces
{
    internal interface IWpfFieldActivator
    {
        void Start(GameField newField, PlayerSide side, IStatusReporter reporter, IResultSender<IGameTurn> sender);

        void Stop();
    }
}
