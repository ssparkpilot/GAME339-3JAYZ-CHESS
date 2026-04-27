using System.Collections.Generic;
using Game339.Shared.Models;
using Game339.Shared.Services;

namespace Game339.Shared.Services.Implementation
{
    public class KnightMovementRule : IChessMovementRule
    {
        private static readonly GridPosition[] Offsets =
        {
            new GridPosition( 2,  1),
            new GridPosition( 2, -1),
            new GridPosition(-2,  1),
            new GridPosition(-2, -1),
            new GridPosition( 1,  2),
            new GridPosition( 1, -2),
            new GridPosition(-1,  2),
            new GridPosition(-1, -2)
        };

        public IEnumerable<GridPosition> GetLegalMoves(
            GridPosition from,
            GridBoard board)
        {
            var moves = new List<GridPosition>();

            foreach (var offset in Offsets)
            {
                var target = new GridPosition(
                    from.X + offset.X,
                    from.Y + offset.Y
                );

                if (!board.IsInBounds(target))
                    continue;

                if (board.GetTile(target).IsOccupied)
                    continue;

                moves.Add(target);
            }

            return moves;
        }
    }
}