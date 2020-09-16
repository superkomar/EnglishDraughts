using System.Threading.Tasks;

using Core.Enums;
using Core.Helpers;
using Core.Interfaces;

namespace Core.Models
{
    public abstract class PlayerBase
    {
        private SingleUseTaskProcessor<IGameTurn> _resultProcessor;
        
        protected ISingleUseResultProcessor<IGameTurn> ResultProcessor { get; }
        
        public void FinishGame(GameField gameField, PlayerSide winner)
        {
            _resultProcessor?.SetResult(null);
            DoFinishGame(gameField, winner);
        }

        public Task<IGameTurn> MakeTurn(GameField gameField)
        {
            _resultProcessor = new SingleUseTaskProcessor<IGameTurn>();
            return DoMakeTurn(gameField);
        }

        public void StartGame(GameField gameField, PlayerSide side) => DoStartGame(gameField, side);

        public void StopMakeTurn() => _resultProcessor?.Cancel();

        protected abstract void DoFinishGame(GameField gameField, PlayerSide winner);

        protected abstract Task<IGameTurn> DoMakeTurn(GameField newField);
        
        protected abstract void DoStartGame(GameField gameField, PlayerSide side);
    }
}
