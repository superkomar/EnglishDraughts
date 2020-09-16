using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Core.Enums;
using Core.Extensions;

[assembly: InternalsVisibleTo("NUnitTests")]
namespace Core.Models
{
    public readonly struct GameField
    {
        private readonly bool _areAnyBlackPieces;
        private readonly bool _areAnyWhitePieces;

        internal GameField(IReadOnlyList<CellState> field, NeighborsHelper neighborsHelper, int dimension)
        {
            Field = field;
            Dimension = dimension;
            NeighborsHelper = neighborsHelper;

            _areAnyBlackPieces = field.Any(x => x.IsSameSide(PlayerSide.Black));
            _areAnyWhitePieces = field.Any(x => x.IsSameSide(PlayerSide.White));
        }

        public int Dimension { get; }
        
        public IReadOnlyList<CellState> Field { get; }
        
        public NeighborsHelper NeighborsHelper { get; }

        public CellState this[int idx] => Field.ElementAtOrDefault(idx);

        public bool AreAnyPieces(PlayerSide side) =>
            side == PlayerSide.Black ? _areAnyBlackPieces : _areAnyWhitePieces;
    }
}
