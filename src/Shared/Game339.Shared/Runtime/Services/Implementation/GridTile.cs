using Game339.Shared.Models;

namespace Game339.Shared.Services.Implementation
{
    public class GridTile
    {
        public GridPosition Position { get; }
        public Unit Occupant { get; private set; }

        public bool IsOccupied => Occupant != null;

        public GridTile(GridPosition position)
        {
            Position = position;
        }

        public void Place(Unit unit)
        {
            Occupant = unit;
            unit.SetPosition(Position);
        }

        public void Clear()
        {
            Occupant = null;
        }
    }
}