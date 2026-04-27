using Game339.Shared.Models;

namespace Game339.Shared.Models
{
    public abstract class Unit
    {
        public GridPosition Position { get; private set; }

        protected Unit(GridPosition position)
        {
            Position = position;
        }

        public void SetPosition(GridPosition newPosition)
        {
            Position = newPosition;
        }
    }
}