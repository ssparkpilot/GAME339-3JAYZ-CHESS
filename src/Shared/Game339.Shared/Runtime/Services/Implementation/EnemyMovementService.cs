using System;
using System.Linq;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;

public class EnemyMovementService
{
    private static readonly Random rng = new Random();
    
    public void ExecuteEnemyMove(EnemyUnit enemy, GridBoard board)
    {
        if (enemy == null || board == null)
            return;

        var legalMoves = enemy.MovementRule.GetLegalMoves(enemy.Position, board).ToList();

        if (legalMoves.Count == 0)
            return;

        var moveTo = legalMoves[rng.Next(legalMoves.Count)]; // random runs outside of unity
        board.MoveUnit(enemy, moveTo);
    }
}