using System.Collections;
using System.Linq;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private float moveInterval = 5f;
    [SerializeField] private float delayBetweenEachEnemyMove = 0.2f;

    private EnemyMovementService movementService;

    private void Awake()
    {
        movementService = new EnemyMovementService();
    }

    private void Start()
    {
        StartCoroutine(MoveEnemiesLoop());
    }

    private IEnumerator MoveEnemiesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(moveInterval);

            var board = BoardManager.main.Board;
            var enemies = board.Enemies.ToList();

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