using UnityEngine;
using Game339.Shared.Services;
using Game339.Shared.Models;

public class EnemyTurnController : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;

    private EnemyMovementService enemyMovementService =
        new EnemyMovementService();

    private void Update()
    {
        var turnManager = TurnManagerBehaviour.main.TurnManager;

        if (!turnManager.CanEnemyAct())
            return;

        var enemies = boardManager.GetAllEnemies();

        enemyMovementService.ExecuteEnemyMoves(
            enemies,
            boardManager.Board
        );

        turnManager.AdvancePhase();
    }
}