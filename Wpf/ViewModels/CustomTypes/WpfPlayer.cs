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
        private readonly IStatusReporter _reporter;

        private SingleUseResultMailbox<IGameTurn> _resultMailbox;
        private PlayerSide _side;
        
        public WpfPlayer(IWpfFieldActivator fieldActivator, IWpfControlsActivator controlsActivator, IStatusReporter reporter)
        {
            _reporter = reporter;
            _fieldActivator = fieldActivator;
            _controlsActivator = controlsActivator;
        }

        public void FinishGame(PlayerSide winner)
        {
            _resultMailbox?.Send(default);
            _reporter?.ReportInfo($"{Resources.WpfPlayer_WinnerIs} {winner}");
        }

        public void InitGame(PlayerSide side)
        {
            _side = side;
        }

        public Task<IGameTurn> MakeTurn(GameField gameField)
        {
            _resultMailbox = new SingleUseResultMailbox<IGameTurn>();

            _fieldActivator.Start(gameField, _side, _reporter, _resultMailbox);
            _controlsActivator.StartTurn();

            return _resultMailbox.ReceiveAsync().ContinueWith(result =>
            {
                _fieldActivator.Stop();
                _controlsActivator.StopTurn();
                return result.Result;
            });
        }
        
        public void StopTurn() => _resultMailbox?.Cancel();
    }
}
