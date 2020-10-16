﻿using System.Threading;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

using Wpf.Interfaces;
using Wpf.Properties;

namespace Wpf.ViewModels.CustomTypes
{
    internal class WpfPlayer : IGamePlayer
    {
        private readonly IWpfControlsActivator _controlsActivator;
        private readonly IWpfFieldActivator _fieldActivator;
        private readonly IReporter _reporter;

        private SingleUseResultChannel<GameTurn> _resultChannel;
        private PlayerSide _side;
        
        public WpfPlayer(IWpfFieldActivator fieldActivator, IWpfControlsActivator controlsActivator, IReporter reporter)
        {
            _reporter = reporter;
            _fieldActivator = fieldActivator;
            _controlsActivator = controlsActivator;
        }

        public void FinishGame(PlayerSide winner)
        {
            _resultChannel?.Send(default);
            _reporter?.ReportStatus($"{Resources.WpfPlayer_WinnerIs} {winner}");
        }

        public void InitGame(PlayerSide side)
        {
            _side = side;
        }

        public Task<GameTurn> MakeTurn(GameField gameField, CancellationToken token)
        {
            _resultChannel = new SingleUseResultChannel<GameTurn>();

            token.Register(() => _resultChannel?.Cancel());

            _fieldActivator.Start(gameField, _side, _reporter, _resultChannel);
            _controlsActivator.StartTurn();

            return _resultChannel.ReceiveAsync().ContinueWith(result =>
            {
                _fieldActivator.Stop();
                _controlsActivator.StopTurn();
                return result.Result;
            });
        }
    }
}
