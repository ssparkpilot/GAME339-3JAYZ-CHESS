using System.Linq;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using UnityEngine;

public class EnemyMovementService
{
    public void ExecuteEnemyMove(EnemyUnit enemy, GridBoard board)
    {
        if (enemy == null || board == null)
            return;

        var legalMoves = enemy.MovementRule.GetLegalMoves(enemy.Position, board).ToList();

        if (legalMoves.Count == 0)
            return;

        var moveTo = legalMoves[Random.Range(0, legalMoves.Count)];
        board.MoveUnit(enemy, moveTo);
    }
}