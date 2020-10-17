using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Core.Enums;
using Core.Extensions;
using Core.Helpers;
using Core.Properties;
using Core.Utils;

[assembly: InternalsVisibleTo("NUnitTests")]
namespace Core.Models
{
    public readonly struct GameField
    {
        private readonly bool _areBlackPiecesPresent;
        private readonly bool _areWhitePiecesPresent;

        private readonly FieldCellCollection _cellCollection;

        internal GameField(IReadOnlyList<CellState> cells, int dimension)
        {
            if (dimension <= 0 || cells == null || !cells.Any())
            {
                throw new ArgumentException(Resources.Error_GameFieldCtor);
            }

            NeighborsFinder = new NeighborsFinder(dimension);
            _cellCollection = new FieldCellCollection(cells, dimension);

            _areBlackPiecesPresent = _cellCollection.Any(x => x.IsSameSide(PlayerSide.Black));
            _areWhitePiecesPresent = _cellCollection.Any(x => x.IsSameSide(PlayerSide.White));
        }

        public GameField(int dimension)
            : this(GameFieldUtils.ConstructInitialField(dimension), dimension)
        { }

        public int GetRowIdx(int cellIdx) => cellIdx / Dimension;

        public int CellCount => _cellCollection.Count;

        public int Dimension => _cellCollection.Dimension;

        public IReadOnlyList<CellState> Cells => _cellCollection;
        
        public NeighborsFinder NeighborsFinder { get; }

        public CellState this[int idx] => Cells.ElementAtOrDefault(idx);

        public bool AreAnyPieces(PlayerSide side) =>
            side == PlayerSide.Black ? _areBlackPiecesPresent : _areWhitePiecesPresent;

        public bool IsValidCellIdx(int cellIdx) => cellIdx > 0 && cellIdx < CellCount;
    }
}
