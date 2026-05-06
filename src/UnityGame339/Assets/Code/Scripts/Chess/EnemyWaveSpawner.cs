using System.Collections;
using UnityEngine;
using Game339.Shared.Models;

public class EnemyWaveSpawner : MonoBehaviour
{
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private int piecesPerWave = 20;

    private void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < piecesPerWave; i++)
        {
            SpawnRandomPiece();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomPiece()
    {
        int i = Random.Range(0, 3);

        if (i == 0)
        {
            SpawnPawnInRandomLane();
        }
        else if (i == 1)
        {
            SpawnKnightInRandomLane();
        }
        else if (i == 2)
        {
            SpawnBishopInRandomLane();
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
    
    private void SpawnKnightInRandomLane()
    {
        int y = Random.Range(0, 8); // 0 to 7
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning knight in lane: " + pos);
            BoardManager.main.SpawnEnemyKnight(pos);
        }
    }
    
    private void SpawnBishopInRandomLane()
    {
        int y = Random.Range(0, 8); // 0 to 7
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning bishop in lane: " + pos);
            BoardManager.main.SpawnEnemyBishop(pos);
        }
    }
}