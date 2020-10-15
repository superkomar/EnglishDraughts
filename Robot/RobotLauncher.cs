using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

using Robot.Interfaces;
using Robot.Models;
using Robot.Properties;

namespace Robot
{
    public class RobotLauncher : IGamePlayer
    {
        private readonly IReporter _reporter;
        private readonly IRobotPlayer _robot;

        public RobotLauncher(int turnTime, IReporter reporter)
        {
            _reporter = reporter;
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

        public void InitGame(PlayerSide side)
        {
            _robot.Init(_reporter, side);
        }
        
        public async Task<IGameTurn> MakeTurn(GameField gameField, CancellationToken token)
        {
            IGameTurn result;

            _reporter?.ReportStatus(Resources.RobotCalculationStatus);

            _reporter?.ReportInfo($"Robot Time: {TurnTime}");

            try
            {
                var cancellationTask = new TaskCompletionSource<IGameTurn>();
                token.Register(() => cancellationTask.TrySetResult(default));

                using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(token);

                var timerTask = Task.Run(async () =>
                {
                    await Task.Delay(TurnTime);

                    tokenSource.Cancel();

                    return _robot.GetTunr();
                });

                result = await await Task.WhenAny(
                    _robot.MakeTurnAsync(gameField, tokenSource.Token),
                    cancellationTask.Task,
                    timerTask)
                    .ConfigureAwait(continueOnCapturedContext: false);
            }
            catch(Exception ex)
            {
                _reporter?.ReportError(ex.ToString());
                result = default;
            }

            _reporter?.ReportInfo($"Result: {(PriorityTurn)result}");

            return result;
        }
    }
}
