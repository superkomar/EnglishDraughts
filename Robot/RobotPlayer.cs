using System;
using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

using Robot.Interfaces;

namespace Robot
{
    public class RobotPlayer : IRobotPlayer
    {
        private PlayerSide _playerSide;

        public RobotPlayer()
        {
        }

        public int TurnTime { get; set; }

        public IGameTurn GetTunr()
        {
            throw new NotImplementedException();
        }

        public void Init(PlayerSide side)
        {
            _playerSide = side;
        }

        public Task<IGameTurn> MakeTurnAsync(GameField newGameField, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
