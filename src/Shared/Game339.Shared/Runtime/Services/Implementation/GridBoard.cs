using System.Collections.Generic;
using Game339.Shared.Models;

namespace Game339.Shared.Services.Implementation
{
    public class GridBoard
    {
        public const int Size = 8;

        private readonly GridTile[,] tiles = new GridTile[Size, Size];

        public GridBoard()
        {
            for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
                tiles[x, y] = new GridTile(new GridPosition(x, y));
        }

        public GridTile GetTile(GridPosition pos)
        {
            if (!IsInBounds(pos))
                return null;

            return tiles[pos.X, pos.Y];
        }

        public bool IsInBounds(GridPosition pos)
        {
            return pos.X >= 0 && pos.X < Size
                              && pos.Y >= 0 && pos.Y < Size;
        }
        
        public void MoveUnit(Unit unit, GridPosition to)
        {
            var fromTile = GetTile(unit.Position);
            var toTile = GetTile(to);

            if (toTile.IsOccupied)
                return; // (later: capture logic)

            fromTile.Clear();
            toTile.Place(unit);
        }
        
        public IEnumerable<GridTile> GetAllTiles()
        {
            for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
                yield return tiles[x, y];
        }
    }
}