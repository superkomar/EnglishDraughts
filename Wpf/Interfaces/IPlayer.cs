using Core.Enums;
using Core.Model;

namespace Wpf.Interfaces
{
    public interface IPlayer
    {
        void Init(int dimension, PlayerSide side);

        GameTurn MakeTurn(GameField gameField);
    }
}
