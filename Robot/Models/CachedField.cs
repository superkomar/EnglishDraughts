using System.Collections.Generic;

using Core.Enums;
using Core.Extensions;
using Core.Models;

using Robot.Extensions;

namespace Robot.Models
{
    internal readonly struct CachedField
    {
        public CachedField(GameField gameField)
        {
            Origin = gameField;

            var blackPieces = new List<int>();
            var whitePieces = new List<int>();

            void Processor(int cellIdx, bool isCellActive)
            {
                if(isCellActive)
                {
                    if      (gameField[cellIdx].IsSameSide(PlayerSide.Black)) blackPieces.Add(cellIdx);
                    else if (gameField[cellIdx].IsSameSide(PlayerSide.White)) whitePieces.Add(cellIdx);
                }
            };

            Origin.ProcessAllCells(Processor);

            BlackPices = blackPieces;
            WhitePices = whitePieces;
        }

        public CellState this[int idx] => Origin[idx];

        public int Dimension => Origin.Dimension;

        public GameField Origin { get; }

        public IReadOnlyList<CellState> Field => Origin.Cells;

        public IReadOnlyList<int> BlackPices { get; }

        public IReadOnlyList<int> WhitePices { get; }

        public IReadOnlyList<int> PiecesBySide(PlayerSide side) =>
            side == PlayerSide.Black ? BlackPices: WhitePices;

        public int PiecesCount(PlayerSide side) => PiecesBySide(side).Count;

        public static implicit operator GameField(CachedField fieldWrapper) => fieldWrapper.Origin;
    }
}
