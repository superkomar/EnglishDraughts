using System.Threading.Tasks;

using Core.Enums;
using Core.Model;

namespace Core.Interfaces
{
    public interface IGamePlayer
    {
        void StartGame(int dimension, PlayerSide side, IStatusReporter statusReporter);

        Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side);

        void EndGame(PlayerSide winner);
    }
}
