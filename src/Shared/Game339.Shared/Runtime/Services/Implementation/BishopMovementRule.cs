using System.Collections.Generic;
using Game339.Shared.Models;
using Game339.Shared.Services;

namespace Game339.Shared.Services.Implementation
{
    public class BishopMovementRule : IChessMovementRule
    {
        private static readonly GridPosition[] Directions =
        {
            new GridPosition( 1,  1),  // up-right
            new GridPosition( 1, -1),  // down-right
            new GridPosition(-1,  1),  // up-left
            new GridPosition(-1, -1)   // down-left
        };

        public IEnumerable<GridPosition> GetLegalMoves(
            GridPosition from,
            GridBoard board)
        {
            var moves = new List<GridPosition>();

            foreach (var dir in Directions)
            {
                var current = from;

                while (true)
                {
                    current = new GridPosition(
                        current.X + dir.X,
                        current.Y + dir.Y
                    );

                    if (!board.IsInBounds(current))
                        break;

                    var tile = board.GetTile(current);

                    if (tile.IsOccupied)
                        break;

                    moves.Add(current);
                }
            }

            return moves;
        }
    }
}