using Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public interface IPlayerLauncher
    {
        IGamePlayer Player { get; }

        IPlayerLauncher Launcher { get; }

        Task<IGameTurn> MakeTurnAsync(Func<IGameTurn> turnFunc, CancellationToken globalToken);
    }

    public interface IPlayerStrategy
    {
        Task<IGameTurn> MakeTurnAsync(Func<IGameTurn> turnFunc, IPlayerParameters parameters, CancellationToken globalToken);
    }

    public class RobotPlayer : IPlayerLauncher
    {
        private const int DefaultRobotCalculationTime = 10000;

        public RobotPlayer(IGamePlayer player)
        {
            Player = player;
        }

        public IGamePlayer Player { get; }

        public IPlayerLauncher Launcher => throw new NotImplementedException();

        public async Task<IGameTurn> MakeTurnAsync(Func<IGameTurn> turnFunc, CancellationToken globalToken)
        {
            var time = Player.Parameters.TurnTime > 0 ? Player.Parameters.TurnTime : DefaultRobotCalculationTime;

            IGameTurn newTurn = await await Task.WhenAny(
                TimerTask<IGameTurn>(time),
                new Task<IGameTurn>(turnFunc));

            return newTurn;
        }

        private static async Task<T> TimerTask<T>(int timerMs)
        {
            await Task.Delay(timerMs);
            return default;
        }
    }

    public class HumanPlayer : IPlayerLauncher
    {
        public HumanPlayer(IGamePlayer player)
        {
            Player = player;
        }

        public IGamePlayer Player { get; }

        public IPlayerLauncher Launcher => throw new NotImplementedException();

        public Task<IGameTurn> MakeTurnAsync(Func<IGameTurn> turnFunc, CancellationToken globalToken)
        {
            IGameTurn newTurn = null;

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
    }

    public class LaunchStrategies
    {
        public enum PlayerType
        {
            Human,
            Robot
        }

        public static IPlayerLauncher GetLauncher(IGamePlayer player, PlayerType type) =>
            type switch
            {
                PlayerType.Robot => new RobotPlayer(player),
                PlayerType.Human => new HumanPlayer(player),
                _ => throw new NotImplementedException()
            };
    }
}
