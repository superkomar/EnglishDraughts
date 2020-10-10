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
            using var cts = new CancellationTokenSource();

            IGameTurn result;

            Console.WriteLine("============================");
            Console.WriteLine($"Robot Time: {TurnTime}");

            try
            {
                var timerTask = Task.Run(async () => {
                    await Task.Delay(TurnTime);
                    cts.Cancel();
                    Console.WriteLine("cts canceled");
                    return _robot.GetTunr();
                });

                result = await await Task.WhenAny(
                    _robot.MakeTurnAsync(gameField, cts.Token),
                    timerTask);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                result = default;
            }

            return result;
        }

        public void StopTurn()
        {
            
        }
    }
}
