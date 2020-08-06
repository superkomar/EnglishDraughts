using Core.Enums;

namespace Core.Model
{
    public class GameCell
    {
        public GameCell(int index, CellState state)
        {
            Index = index;
            State = state;
        }

        public int Index { get; }

        public CellState State { get; set; }
    }
}
