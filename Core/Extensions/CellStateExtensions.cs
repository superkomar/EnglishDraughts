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
        /// <param name="state"></param>
        /// <returns></returns>
        public static PlayerSide ToPlayerSide(this CellState state) =>
            state switch
            {
                var x when x == CellState.BlackMen || x == CellState.BlackKing => PlayerSide.Black,
                var x when x == CellState.WhiteMen || x == CellState.WhiteKing => PlayerSide.White,
                _ => PlayerSide.None
            };
    }
}
