using Core.Enums;

namespace Core.Models
{
    public readonly struct GameState
    {
        public GameState(GameField field, StateType state, PlayerSide side)
        {
            Field = field;
            State = state;
            Side = side;
        }

        public GameField Field { get; }

        public StateType State { get; }

        public PlayerSide Side { get; }
    }
}
