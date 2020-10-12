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

        private readonly FieldCellCollection _cellCollection;

        internal GameField(IReadOnlyList<CellState> cells, NeighborsHelper neighborsHelper, int dimension)
        {
            _cellCollection = new FieldCellCollection(cells, dimension);

            NeighborsHelper = neighborsHelper;

            _areAnyBlackPieces = _cellCollection.Any(x => x.IsSameSide(PlayerSide.Black));
            _areAnyWhitePieces = _cellCollection.Any(x => x.IsSameSide(PlayerSide.White));

        }

        internal GameField(IReadOnlyList<CellState> cells, GameField other)
        {
            _cellCollection = new FieldCellCollection(cells, other.Dimension);

            NeighborsHelper = other.NeighborsHelper;

            _areAnyBlackPieces = _cellCollection.Any(x => x.IsSameSide(PlayerSide.Black));
            _areAnyWhitePieces = _cellCollection.Any(x => x.IsSameSide(PlayerSide.White));
        }

        public int CellsCount => _cellCollection.Count;
        
        public int Dimension => _cellCollection.Dimension;

        public IReadOnlyList<CellState> Field => _cellCollection;
        
        public NeighborsHelper NeighborsHelper { get; }

        public CellState this[int idx] => Field.ElementAtOrDefault(idx);

        public bool AreAnyPieces(PlayerSide side) =>
            side == PlayerSide.Black ? _areAnyBlackPieces : _areAnyWhitePieces;

        public bool IsValidCellIdx(int cellIdx) => cellIdx > 0 && cellIdx < CellsCount;
    }
}
