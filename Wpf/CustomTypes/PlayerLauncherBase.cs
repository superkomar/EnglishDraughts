using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Model;
using Core.Utils;

namespace Wpf.CustomTypes
{
    public abstract class PlayerLauncherBase : IPlayerLauncher
    {
        public PlayerLauncherBase(IGamePlayer player)
        {
            Player = player;
        }

        public IGamePlayer Player { get; }

        public abstract Task<IGameTurn> MakeTurnAsync(IModelController modelController, PlayerSide side);

        public void FinishGame(PlayerSide winner) => TaskProcessor?.Set(null);

        public void InitGame(IModelController modelController, PlayerSide side, IStatusReporter reporter) =>
            Player.InitGame(modelController.Dimension, side, reporter);

        protected OneshotTaskProcessor<IGameTurn> TaskProcessor;
    }
}
