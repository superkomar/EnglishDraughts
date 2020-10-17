using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Models;

namespace Robot.Interfaces
{
    public interface IRobotPlayer
    {
        int TurnTime { get; set; }

        /// <summary>
        /// Call if time is up.
        /// </summary>
        /// <returns> Get a better turn or default</returns>
        GameTurn GetTunr();

        Task<GameTurn> MakeTurnAsync(GameField gameField, PlayerSide side, CancellationToken token);
    }
}
