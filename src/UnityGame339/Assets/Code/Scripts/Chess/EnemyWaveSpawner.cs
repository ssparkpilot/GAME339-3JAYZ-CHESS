using System.Collections;
using UnityEngine;
using Game339.Shared.Models;

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private int pawnsPerWave = 20;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < pawnsPerWave; i++)
        {
            SpawnPawnInRandomLane();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnPawnInRandomLane()
    {
        int y = Random.Range(0, 8); // 0 to 7
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning pawn in lane: " + pos);
            BoardManager.main.SpawnEnemyPawn(pos);
        }
    }
}