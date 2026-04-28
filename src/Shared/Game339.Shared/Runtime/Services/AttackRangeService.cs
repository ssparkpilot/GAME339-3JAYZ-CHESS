using System.Collections.Generic;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;

namespace Game339.Shared.Services
{
    public class AttackRangeService
    {
        public IEnumerable<EnemyUnit> GetEnemiesInRange( 
            TowerUnit tower, 
            GridBoard board)
        {
            var results = new List<EnemyUnit>();

            for (int x = 0; x < GridBoard.Size; x++)
            for (int y = 0; y < GridBoard.Size; y++)
            {
                var tile = board.GetTile(new GridPosition(x, y));

                if (tile.IsOccupied && tile.Occupant is EnemyUnit enemy)
                {
                    if (ManhattanDistance(tower.Position, enemy.Position) <= tower.Range)
                        results.Add(enemy);
                }
            }

            return results; 
        }
        private int ManhattanDistance(GridPosition a, GridPosition b)
        {
            return System.Math.Abs(a.X - b.X) + System.Math.Abs(a.Y - b.Y);
        }
    }
}