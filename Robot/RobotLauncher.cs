using System;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

using NLog;

using Robot.Interfaces;
using Robot.Models;
using Robot.Properties;

namespace Robot
{
    public class RobotLauncher : IGamePlayer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IStatusReporter _statusReporter;
        private readonly IRobotPlayer _robot;

        private PlayerSide _playerSide;

        public RobotLauncher(int turnTime, IStatusReporter statusReporter)
        {
            _statusReporter = statusReporter;
            _robot = new RobotPlayer();

            TurnTime = turnTime;
        }

        public int TurnTime
        {
            get => _robot.TurnTime;
            set => _robot.TurnTime = value;
        }

        public void FinishGame(PlayerSide winner)
        { }

        public void InitGame(PlayerSide side) => _playerSide = side;
        
        public async Task<GameTurn> MakeTurn(GameField gameField, CancellationToken token)
        {
            GameTurn result;

            _statusReporter.Status = Resources.RobotCalculationStatus;

            try
            {
                var cancellationTask = new TaskCompletionSource<GameTurn>();
                token.Register(() => cancellationTask.TrySetResult(default));

                using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

                var timerTask = Task.Run(async () =>
                {
                    await Task.Delay(TurnTime);

                    tokenSource.Cancel();

                    return _robot.GetTunr();
                });

                result = await await Task.WhenAny(
                    _robot.MakeTurnAsync(gameField, _playerSide, tokenSource.Token),
                    cancellationTask.Task,
                    timerTask)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch(Exception ex)
            {
                Logger.Error(ex);

                result = default;
            }

            return result;
        }
    }
}
