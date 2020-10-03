using System.Threading.Tasks;

using Core.Enums;
using Core.Models;

namespace Core.Interfaces
{
    public interface IGamePlayer
    {
        void FinishGame(PlayerSide winner);

        void InitGame(PlayerSide side);

        Task<IGameTurn> MakeTurn(GameField gameField);

        void StopTurn();
    }
}
