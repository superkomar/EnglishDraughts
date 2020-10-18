﻿using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
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
        /// <returns>First founded turn or default</returns>
        IGameTurn GetTunr();

        Task<IGameTurn> MakeTurnAsync(GameField newGameField, CancellationToken token);
    }
}
