using System;
using System.Threading.Tasks;

using Core;
using Core.Enums;
using Core.Interfaces;
using Core.Model;

namespace Robot
{
    public class RobotPlayer : IGamePlayer
    {
        public int TurnTime { get; set; }

        public IPlayerParameters Parameters => throw new NotImplementedException();

        public void FinishGame(PlayerSide winner)
        {
            throw new NotImplementedException();
        }

        public void InitGame(int dimension, PlayerSide side, IStatusReporter statusReporter)
        {
            throw new NotImplementedException();
        }

        public async Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side)
        {
            throw new NotImplementedException();
        }

        public Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side, TaskCompletionSource<IGameTurn> tcs)
        {
            throw new NotImplementedException();
        }

        public Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side, IOneshotTaskProcessor<IGameTurn> taskProcessor)
        {
            throw new NotImplementedException();
        }
    }
}
