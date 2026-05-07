using System.Collections.Generic;
using Game339.Shared.Models;

namespace Game339.Shared.Services.Implementation
{
    public class QueenMovementRule : IChessMovementRule
    {
        private readonly RookMovementRule rookRule = new RookMovementRule();
        private readonly BishopMovementRule bishopRule = new BishopMovementRule();

        public IEnumerable<GridPosition> GetLegalMoves(
            GridPosition from,
            GridBoard board)
        {
            var moves = new List<GridPosition>();

            moves.AddRange(rookRule.GetLegalMoves(from, board));
            moves.AddRange(bishopRule.GetLegalMoves(from, board));

            return moves;
        }
    }
}