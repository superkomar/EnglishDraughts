using System;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Robot
{
    public class RobotPlayer : PlayerBase
    {
        private const int DefaultRobotCalculationTime = 10000;

        public int TurnTime { get; set; }

        public IPlayerParameters Parameters => throw new NotImplementedException();

        protected override void DoFinishGame(GameField gameField, PlayerSide winner)
        {
            throw new NotImplementedException();
        }

        protected override Task<IGameTurn> DoMakeTurn(GameField newField)
        {
            return default;

            var time = TurnTime > 0 ? TurnTime : DefaultRobotCalculationTime;

            //return await await Task.WhenAny(
            //    Player.MakeTurnAsync(gameField, ResultProcessor),
            //    TimerTask<IGameTurn>(time));
        }

        private static async Task<T> TimerTask<T>(int timerMs)
        {
            await Task.Delay(timerMs);
            return default;
        }

        protected override void DoStartGame(GameField gameField, PlayerSide side)
        {
            throw new NotImplementedException();
        }
    }
}
