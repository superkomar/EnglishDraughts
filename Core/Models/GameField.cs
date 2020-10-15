using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Core.Enums;
using Core.Extensions;
using Core.Helpers;

[assembly: InternalsVisibleTo("NUnitTests")]
namespace Core.Models
{
    public readonly struct GameField
    {
        private readonly bool _areBlackPiecesPresent;
        private readonly bool _areWhitePiecesPresent;

        private readonly FieldCellCollection _cellCollection;

        internal GameField(IReadOnlyList<CellState> cells, NeighborsFinder neighborsHelper, int dimension)
        {
            _cellCollection = new FieldCellCollection(cells, dimension);

            NeighborsFinder = neighborsHelper;

            _areBlackPiecesPresent = _cellCollection.Any(x => x.IsSameSide(PlayerSide.Black));
            _areWhitePiecesPresent = _cellCollection.Any(x => x.IsSameSide(PlayerSide.White));
        }

        internal GameField(IReadOnlyList<CellState> cells, GameField other)
        {
            _cellCollection = new FieldCellCollection(cells, other.Dimension);

            NeighborsFinder = other.NeighborsFinder;

            _areBlackPiecesPresent = _cellCollection.Any(x => x.IsSameSide(PlayerSide.Black));
            _areWhitePiecesPresent = _cellCollection.Any(x => x.IsSameSide(PlayerSide.White));
        }

        public int GetRowIdx(int cellIdx) => cellIdx / Dimension;

        public int CellCount => _cellCollection.Count;

        public int Dimension => _cellCollection.Dimension;

        public IReadOnlyList<CellState> Field => _cellCollection;
        
        public NeighborsFinder NeighborsFinder { get; }

        public CellState this[int idx] => Field.ElementAtOrDefault(idx);

        public bool AreAnyPieces(PlayerSide side) =>
            side == PlayerSide.Black ? _areBlackPiecesPresent : _areWhitePiecesPresent;

        public bool IsValidCellIdx(int cellIdx) => cellIdx > 0 && cellIdx < CellCount;
    }
}
