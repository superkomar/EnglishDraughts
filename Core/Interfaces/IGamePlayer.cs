﻿using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Models;

namespace Core.Interfaces
{
    public interface IGamePlayer
    {
        void FinishGame(PlayerSide winner);

        void InitGame(PlayerSide side);

        Task<GameTurn> MakeTurn(GameField gameField, CancellationToken token);
    }
}
