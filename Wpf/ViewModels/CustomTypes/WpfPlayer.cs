using System;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Models;

namespace Wpf.ViewModels.CustomTypes
{
    internal class WpfPlayer : PlayerBase
    {
        private readonly IWpfTurnWaiter _turnWaiter;
        private readonly IStatusReporter _reporter;

        private int _dimension;
        private PlayerSide _side;

        public WpfPlayer(IWpfTurnWaiter turnWaiter, IStatusReporter reporter)
        {
            _reporter = reporter;
            _turnWaiter = turnWaiter;
        }

        public IPlayerParameters Parameters => throw new NotImplementedException();

        protected override void DoFinishGame(GameField gameField, PlayerSide winner)
        {
            _reporter?.ReportInfo($"Winner is {winner}");
        }

        protected override Task<IGameTurn> DoMakeTurn(GameField gameField)
        {
            if (gameField.Dimension != _dimension)
            {
                throw new ArgumentException("Failed!!! Incorrect game field");
            }

            _turnWaiter.Start(gameField, _side, _reporter, ResultProcessor);

            return ResultProcessor.WaitAsync().ContinueWith(result =>
            {
                _turnWaiter.Stop();
                return result.Result;
            });
        }

        protected override void DoStartGame(GameField gameField, PlayerSide side)
        {
            _dimension = gameField.Dimension;
            _side = side;
        }
    }
}
