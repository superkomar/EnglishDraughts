using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Model;
using Core.Utils;

namespace Wpf.CustomTypes
{
    public sealed class HumanLauncher : PlayerLauncherBase
    {
        public HumanLauncher(IGamePlayer player)
            : base(player)
        { }

        public override async Task<IGameTurn> MakeTurnAsync(IModelController modelController, PlayerSide side)
        {
            IGameTurn turn = null;

            bool isHistoryRolled = false;

            void OnHistoryChanged(object sender, System.EventArgs e)
            {
                isHistoryRolled = true;
                TaskProcessor.Cancel();
            };

            modelController.HistoryRolling += OnHistoryChanged;

            do
            {
                isHistoryRolled = false;
                TaskProcessor = new OneshotTaskProcessor<IGameTurn>();

                turn = await Player.MakeTurnAsync(modelController.Field, side, TaskProcessor);
            } while (isHistoryRolled);

            modelController.HistoryRolling -= OnHistoryChanged;

            return turn;
        }
    }
}
