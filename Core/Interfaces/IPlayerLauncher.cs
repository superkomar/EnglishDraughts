using System.Threading.Tasks;

using Core.Enums;
using Core.Model;

namespace Core.Interfaces
{
    public interface IPlayerLauncher
    {
        IGamePlayer Player { get; }

        void FinishGame();

        void InitGame(int dimension, PlayerSide white, IStatusReporter reporter);

        Task<IGameTurn> MakeTurnAsync(GameField gameField, PlayerSide side);
    }
}
