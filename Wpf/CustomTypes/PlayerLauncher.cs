using System.Threading;
using System.Threading.Tasks;

using Core;
using Core.Enums;
using Core.Interfaces;
using Core.Model;

namespace Wpf.CustomTypes
{
    public abstract class PlayerLauncher : IPlayerLauncher
    {
        public PlayerLauncher(IGamePlayer player)
        {
            Player = player;
        }

        public IGamePlayer Player { get; }

        public abstract Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side);

        public void FinishGame() => TaskProcessor?.Set(null);

        public void InitGame(int dimension, PlayerSide side, IStatusReporter reporter) =>
            Player.InitGame(dimension, side, reporter);

        protected OneshotTaskProcessor<IGameTurn> TaskProcessor;
    }

    public sealed class RobotPlayer : PlayerLauncher
    {
        private const int DefaultRobotCalculationTime = 10000;

        public RobotPlayer(IGamePlayer player)
            : base(player)
        { }

        private static async Task<T> TimerTask<T>(int timerMs)
        {
            await Task.Delay(timerMs);
            return default;
        }

        public override async Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side)
        {
            TaskProcessor = new OneshotTaskProcessor<IGameTurn>();

            var time = Player.Parameters.TurnTime > 0 ? Player.Parameters.TurnTime : DefaultRobotCalculationTime;

            return await await Task.WhenAny(
                Player.MakeTurnAsync(gameField, side, TaskProcessor),
                TimerTask<IGameTurn>(time));
        }
    }

    public sealed class HumanPlayer : PlayerLauncher
    {
        public HumanPlayer(IGamePlayer player)
            : base(player)
        { }

        public async Task<IGameTurn> LaunchAsync(Task<IGameTurn> turnTask, CancellationToken globalToken)
        {
            IGameTurn newTurn = await turnTask;

            return newTurn;

            //bool repeatTurn;

            //do
            //{
            //    var localTokenSource = new CancellationTokenSource();

            //    void HistoryHandler(object sender, EventArgs e) => _cancellationSource?.Cancel();

            //    repeatTurn = false;
            //    _cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(globalToken, localTokenSource.Token);

            //    try
            //    {
            //        _modelController.HistoryRolling += HistoryHandler;
            //        newTurn = await Task.Run(turnFunc, _cancellationSource.Token);
            //    }
            //    catch (TaskCanceledException)
            //    {
            //        repeatTurn = true;
            //    }
            //    catch (Exception)
            //    {
            //        repeatTurn = false;
            //    }
            //    finally
            //    {
            //        _modelController.HistoryRolling -= HistoryHandler;
            //    }

            //} while (repeatTurn);

            return null;
        }

        public override async Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side)
        {
            TaskProcessor = new OneshotTaskProcessor<IGameTurn>();

            return await Player.MakeTurnAsync(gameField, side, TaskProcessor);
        }
    }
}
