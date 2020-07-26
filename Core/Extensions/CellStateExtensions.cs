using Core.Enums;

namespace Core.Extensions
{
    public static class CellStateExtensions
    {
        public static bool IsKing(this CellState state) =>
            state == CellState.BlackKing || state == CellState.WhiteKing;

        public static bool IsOpposite(this CellState src, CellState dst) =>
            src.ToPlayerSide() != dst.ToPlayerSide();

        public static CellState LevelUp(this CellState state) =>
            state switch
            {
                CellState.BlackMen => CellState.BlackKing,
                CellState.WhiteMen => CellState.WhiteKing,
                _ => state
            };

        /// <summary>
        /// Convert CellState to the PlayerSide.
        /// Empty convert to Black.
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static PlayerSide ToPlayerSide(this CellState src) =>
            src == CellState.WhiteKing || src == CellState.WhiteMen ? PlayerSide.White : PlayerSide.Black;
    }
}
