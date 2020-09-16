using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Model;
using Core.Utils;

namespace Wpf.CustomTypes
{
    internal sealed class RobotLauncher : PlayerLauncherBase
    {
        private const int DefaultRobotCalculationTime = 10000;

        public RobotLauncher(IGamePlayer player)
            : base(player)
        { }

        private static async Task<T> TimerTask<T>(int timerMs)
        {
            await Task.Delay(timerMs);
            return default;
        }

        public override async Task<IGameTurn> MakeTurnAsync(IModelController modelController, PlayerSide side)
        {
            TaskProcessor = new OneshotTaskProcessor<IGameTurn>();

            var time = Player.Parameters.TurnTime > 0 ? Player.Parameters.TurnTime : DefaultRobotCalculationTime;

            return await await Task.WhenAny(
                Player.MakeTurnAsync(modelController.Field, side, TaskProcessor),
                TimerTask<IGameTurn>(time));
        }
    }
}
