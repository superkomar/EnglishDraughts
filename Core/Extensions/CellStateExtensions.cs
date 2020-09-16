using Core.Enums;

namespace Core.Extensions
{
    public static class CellStateExtensions
    {
        public static bool IsKing(this CellState state) =>
            state == CellState.BlackKing || state == CellState.WhiteKing;

        public static bool IsOpposite(this CellState src, CellState dst) =>
            (src.IsWhitePiece() && dst.IsBlackPiece()) || (src.IsBlackPiece() && dst.IsWhitePiece());

        public static bool IsSameSide(this CellState state, PlayerSide side) =>
            side switch
            {
                PlayerSide.White => state.IsWhitePiece(),
                PlayerSide.Black => state.IsBlackPiece(),
                _ => false
            };

        public static CellState LevelUp(this CellState state) =>
            state switch
            {
                CellState.BlackMen => CellState.BlackKing,
                CellState.WhiteMen => CellState.WhiteKing,
                _ => state
            };

        public static bool TryGetPlayerSide(this CellState state, out PlayerSide side)
        {
            side = default;

            if (state.IsWhitePiece())
            {
                side = PlayerSide.White;
                return true;
            }
            else if (state.IsBlackPiece())
            {
                side = PlayerSide.Black;
                return true;
            }

            return false;
        }
        
        private static bool IsBlackPiece(this CellState cell) =>
            cell != CellState.Empty && (cell == CellState.BlackKing || cell == CellState.BlackMen);

        private static bool IsWhitePiece(this CellState cell) =>
                    cell != CellState.Empty && (cell == CellState.WhiteKing || cell == CellState.WhiteMen);
    }
}
