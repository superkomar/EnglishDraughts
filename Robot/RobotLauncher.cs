using System;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

using Robot.Interfaces;

namespace Robot
{
    public class RobotLauncher : IGamePlayer
    {
        private readonly IReporter _reporter;
        private readonly IRobotPlayer _robot;

        private CancellationTokenSource _tokenSource;

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
        
        public async Task<IGameTurn> MakeTurn(GameField gameField)
        {
            using var cts = new CancellationTokenSource();

            IGameTurn result;

            _reporter?.ReportStatus("Robot is searchin a turn");

            _reporter?.ReportInfo($"=== Robot Time: {TurnTime}");

            try
            {
                var timerTask = Task.Run(async () => {
                    await Task.Delay(TurnTime);
                    
                    cts.Cancel();

                    return _robot.GetTunr();
                });

                result = await await Task.WhenAny(
                    _robot.MakeTurnAsync(gameField, cts.Token),
                    timerTask);
            }
            catch(Exception ex)
            {
                _reporter?.ReportError(ex.ToString());
                result = default;
            }

            return result;
        }

        public void StopTurn()
        {
            _tokenSource?.Cancel();
        }
    }
}
