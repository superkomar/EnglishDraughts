using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Models;

namespace Robot.Interfaces
{
    public interface IRobotPlayer
    {
        int TurnTime { get; set; }

        void Init(PlayerSide side);

        /// <summary>
        /// Call if time is up.
        /// </summary>
        /// <returns>Best founded turn or default</returns>
        GameTurn GetTunr();

        Task<GameTurn> MakeTurnAsync(GameField gameField, CancellationToken token);
    }
}
