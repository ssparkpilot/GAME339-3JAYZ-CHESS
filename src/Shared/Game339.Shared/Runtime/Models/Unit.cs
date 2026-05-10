using Game339.Shared.Models;

namespace Game339.Shared.Models
{
    public abstract class Unit
    {
        public GridPosition Position { get; private set; }
        public bool IsEnemy { get; private set; }

        protected Unit(GridPosition position, bool isEnemy)
        {
            Position = position;
            IsEnemy = isEnemy;
        }

        public void SetPosition(GridPosition newPosition)
        {
            Position = newPosition;
        }

        public void SetIsEnemy(bool isEnemy)
        {
            IsEnemy = isEnemy;
        }
    }
    }
