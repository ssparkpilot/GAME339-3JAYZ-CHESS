using System.Collections;
using System.Linq;
using UnityEngine;
using Game339.Shared.Services;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private float moveInterval = 5f;

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
            
            var enemies = BoardManager.main.Board.Enemies.ToList();
            
            movementService.ExecuteEnemyMoves(
                enemies,
                BoardManager.main.Board
            );

            foreach (var view in FindObjectsOfType<EnemyView>())
            {
                view.UpdatePosition();
            }
        }
    }
}