using UnityEngine;
using Game339.Shared.Models;

public class EnemySpawnDebug : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Spawning pawn...");
        BoardManager.main.SpawnEnemyPawn(new GridPosition(0, 0));
    }
}