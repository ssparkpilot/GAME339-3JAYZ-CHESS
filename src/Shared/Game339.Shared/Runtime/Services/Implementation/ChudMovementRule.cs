using System.Collections.Generic;
using Game339.Shared.Models;
using Game339.Shared.Services;

namespace Game339.Shared.Services.Implementation
{
    public class ChudMovementRule : IChessMovementRule
    {
        public IEnumerable<GridPosition> GetLegalMoves(
            GridPosition from,
            GridBoard board)
        {
            var moves = new List<GridPosition>();

            // Pawns move right (enemies advance from left)
            var forward = new GridPosition(from.X - 1, from.Y);

            // Must be inside board
            if (!board.IsInBounds(forward))
                return moves;

            // Must be empty (no jumping)
            

            moves.Add(forward);
            return moves;
        }
    }
}
