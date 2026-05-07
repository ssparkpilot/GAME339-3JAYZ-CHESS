using System.Collections;
using System.Linq;
using Game.Runtime;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private float delayBetweenEachEnemyMove = 0.2f;

    private EnemyMovementService movementService;
    private TurnManager turnManager;

    private void Awake()
    {
        movementService = new EnemyMovementService();
        turnManager = ServiceResolver.Resolve<TurnManager>();
    }

    private void Start()
    {
        turnManager.OnTurnStateChanged += HandlePhaseChange;
    }
    
    private void OnDestroy()
    {
        turnManager.OnTurnStateChanged -= HandlePhaseChange;
    }
    
    private void HandlePhaseChange(TurnOwner owner, TurnPhase phase)
    {
        if (phase == TurnPhase.EnemyMoving)
        {
            StartCoroutine(RunEnemyTurn());
        }
    }
    
    private IEnumerator RunEnemyTurn()
    {
        turnManager.SetBusy(true);
            
        var board = BoardManager.main.Board;

        var enemies = board.Enemies
            .OrderByDescending(e => e.Position.X)
            .ThenByDescending(e => e.Position.Y)
            .ToList();

        foreach (var enemy in enemies)
        {
            if (enemy == null)
                continue;

            var tile = board.GetTile(enemy.Position);
            if (tile == null || tile.Occupant != enemy)
                continue;

            movementService.ExecuteEnemyMove(enemy, board);

            foreach (var view in FindObjectsOfType<EnemyView>())
            {
                view.UpdatePosition();
            }

            yield return new WaitForSeconds(delayBetweenEachEnemyMove);
        }

        // end enemy turn after all enemies move
        turnManager.SetBusy(false);
        turnManager.AdvancePhase();
    }

    private IEnumerator MoveEnemiesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            // moves rightmost enemies first
            var board = BoardManager.main.Board;
            var enemies = BoardManager.main.Board.Enemies
                .OrderByDescending(e => e.Position.X)
                .ThenByDescending(e => e.Position.Y)
                .ToList();

            foreach (var enemy in enemies)
            {
                if (enemy == null)
                    continue;

                var tile = board.GetTile(enemy.Position);
                if (tile == null || tile.Occupant != enemy)
                    continue;

                movementService.ExecuteEnemyMove(enemy, board);

                foreach (var view in FindObjectsOfType<EnemyView>())
                {
                    view.UpdatePosition();
                }

                yield return new WaitForSeconds(delayBetweenEachEnemyMove);
            }
        }
    }
}