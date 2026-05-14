using System.Collections.Generic;
using Game339.Shared.Models;
using Game339.Shared.Services;

namespace Game339.Shared.Services.Implementation
{
    public class KingMovementRule : IChessMovementRule
    {
        public IEnumerable<GridPosition> GetLegalMoves(
            GridPosition from,
            GridBoard board)
        {
            var moves = new List<GridPosition>();

            // King move right (enemies advance from left)
            var forward = new GridPosition(from.X, from.Y);

            moves.Add(forward);
            return moves;
        }
    }
}