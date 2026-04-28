using Game339.Shared.Services;
using Game339.Shared.Models;

namespace Game339.Shared.Models
{
    public class EnemyUnit : Unit
    {
        public IChessMovementRule MovementRule { get; }

        public EnemyUnit(GridPosition startPos, IChessMovementRule rule)
            : base(startPos)
        {
            MovementRule = rule;
        }
    }
}