using System.Collections.Generic;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;

namespace Game339.Shared.Services
{
    public interface IChessMovementRule
    {
        IEnumerable<GridPosition> GetLegalMoves(
            GridPosition from,
            GridBoard board
        );
    }
}