using System.Threading.Tasks;

using Core.Enums;
using Core.Models;

namespace Core.Interfaces
{
    public interface IPlayerParameters
    {
        int TurnTime { get; }
    }

    public interface IGamePlayer
    {
        IPlayerParameters Parameters { get; }

        void InitGame(GameField gameField, PlayerSide side, IStatusReporter statusReporter);

        Task<IGameTurn> MakeTurn(GameField gameField, ISingleUseResultProcessor<IGameTurn> taskProcessor);

        void FinishGame(GameField gameField, PlayerSide winner);
    }
}
