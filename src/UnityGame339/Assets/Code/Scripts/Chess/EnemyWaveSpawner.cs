using System.Collections;
using Game.Runtime;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;

public class EnemyWaveSpawner : MonoBehaviour
{
    private TurnManager turnManager;

    private int kingCountDown = 1;

    void Start()
    {
        turnManager = ServiceResolver.Resolve<TurnManager>();
        turnManager.OnPhaseChanged += HandlePhase;
    }
    
    private void HandlePhase(TurnPhase phase)
    {
        if (phase == TurnPhase.EnemyTurnStart)
        {
            if (kingCountDown <= 10 && kingCountDown > 0)
            {
                SpawnTurnEnemies();
            }
            else
            {
                if (kingCountDown != 0)
                {
                    SpawnKingIn();
                    kingCountDown = 0;
                }
            }
        }
    }
    
    void SpawnTurnEnemies()
    {
        int spawnCount = 4; // per turn (change this!!!)

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnRandomPiece();
            kingCountDown++;
        }
    }

    private void SpawnRandomPiece()
    {
        int i = Random.Range(0, 5);

        if (i == 0)
            SpawnPawnInRandomLane();
        else if (i == 1)
            SpawnKnightInRandomLane();
        else if (i == 2)
            SpawnBishopInRandomLane();
        else if (i == 3)
            SpawnRookInRandomLane();
        else if (i == 4)
            SpawnQueenInRandomLane();
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
    
    private void SpawnRookInRandomLane()
    {
        int y = Random.Range(0, 8); // 0 to 7
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning rook in lane: " + pos);
            BoardManager.main.SpawnEnemyRook(pos);
        }
    }
    
    private void SpawnQueenInRandomLane()
    {
        int y = Random.Range(0, 8); // 0 to 7
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning queen in lane: " + pos);
            BoardManager.main.SpawnEnemyQueen(pos);
        }
    }
    
    private void SpawnKingIn()
    {
        var pos = new GridPosition(0, 4);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning KING in lane: " + pos);
            BoardManager.main.SpawnEnemyKing(pos);
        }
    }
    
    private void OnDestroy()
    {
        if (turnManager != null)
        {
            turnManager.OnPhaseChanged -= HandlePhase;
        }
    }
}