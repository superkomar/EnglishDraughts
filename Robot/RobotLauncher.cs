using System;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

using Robot.Interfaces;

namespace Robot
{
    internal class ExecutionTimer
    {
        private readonly Func<IGameTurn> _resultGetter;

        public ExecutionTimer(int timerMs, Func<IGameTurn> resultGetter)
        {
            TimeMs = timerMs;

            _resultGetter = resultGetter ?? (() => default);
        }

        public int TimeMs { get; }

        public Task<IGameTurn> GetTimerTask()
        {
            return Task.Delay(TimeMs).ContinueWith(_ => _resultGetter());
        }
    }

    public class RobotLauncher : IGamePlayer
    {
        private readonly IRobotPlayer _robot;

        public RobotLauncher(int turnTime)
        {
            _robot = new RobotPlayer();

            TurnTime = turnTime;
        }

        public int TurnTime
        {
            get => _robot.TurnTime;
            set => _robot.TurnTime = value;
        }

        public void FinishGame(PlayerSide winner)
        {
        }

        public void InitGame(PlayerSide side)
        {
            _robot.Init(side);
        }

        public async Task<IGameTurn> MakeTurn(GameField gameField)
        {
            var cts = new CancellationTokenSource(TurnTime);

            var timerTask = Task.Delay(TurnTime).ContinueWith(_ => _robot.GetTunr());

            return await await Task.WhenAny(timerTask, _robot.MakeTurnAsync(gameField, cts.Token));
        }

        public void StopTurn()
        {
            throw new NotImplementedException();
        }
    }
}
