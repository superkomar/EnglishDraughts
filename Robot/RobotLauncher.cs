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
            using var cts = new CancellationTokenSource(TurnTime);

            var timerTask = Task.Delay(TurnTime).ContinueWith(_ => _robot.GetTunr());

            IGameTurn result;

            try
            {
                result = await await Task.WhenAny(timerTask, _robot.MakeTurnAsync(gameField, cts.Token));
            }
            catch(Exception)
            {
                result = default;
            }

            return result;
        }

        public void StopTurn()
        {
            
        }
    }
}
