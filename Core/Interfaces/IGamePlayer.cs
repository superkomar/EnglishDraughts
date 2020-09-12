using System.Threading.Tasks;

using Core.Enums;
using Core.Model;

namespace Core.Interfaces
{
    public interface IPlayerParameters
    {
        int TurnTime { get; }
    }

    public interface IGamePlayer
    {
        IPlayerParameters Parameters { get; }

        void InitGame(int dimension, PlayerSide side, IStatusReporter statusReporter);

        Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side, IOneshotTaskProcessor<IGameTurn> taskProcessor);

        void FinishGame(PlayerSide winner);
    }
}
