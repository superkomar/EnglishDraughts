using System;
using System.Threading.Tasks;

using Core.Enums;
using Core.Interfaces;
using Core.Model;

namespace Wpf.ViewModels.CustomTypes
{
    internal class WpfPlayer : IGamePlayer
    {
        private readonly IInterface1 _updater;
        
        private IStatusReporter _reporter;
        private PlayerSide _side;

        public WpfPlayer(IInterface1 updater)
        {
            _updater = updater;
        }

        public IPlayerParameters Parameters => throw new NotImplementedException();

        public void FinishGame(PlayerSide winner)
        {
            throw new NotImplementedException();
        }

        public void InitGame(int dimension, PlayerSide side, IStatusReporter reporter)
        {
            _dimension = dimension;
            _reporter = reporter;
            _side = side;
        }

        private int _dimension;

        public async Task<IGameTurn> MakeTurnAsync(GameField gameField, IOneshotTaskProcessor<IGameTurn> taskProcessor)
        {
            if (gameField.Dimension != _dimension) throw new ArgumentException("Failed!!! Incorrect game field");

            _updater.IsActive = true;

            _updater.Update(gameField, _side, _reporter, taskProcessor);

            IGameTurn turn = await taskProcessor.Get();

            _updater.IsActive = false;

            return turn;
        }
    }
}
