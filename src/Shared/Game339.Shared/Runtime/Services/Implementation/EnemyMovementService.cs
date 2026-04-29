using System.Collections.Generic;
using System.Linq;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;

namespace Game339.Shared.Services
{
    public class EnemyMovementService
    {
        public void ExecuteEnemyMoves(
            IEnumerable<EnemyUnit> enemies,
            GridBoard board)
        {
            var enemyList = enemies.ToList();
                
            foreach (var enemy in enemyList)
            {
                var moves = enemy.MovementRule
                    .GetLegalMoves(enemy.Position, board)
                    .ToList();

                if (moves.Count == 0)
                    continue;

                //  Simple AI: first legal move
                var chosenMove = moves[0];

                board.MoveUnit(enemy, chosenMove);
            }
        }
    }
}