using System.Collections;
using Game.Runtime;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;

public class EnemyWaveSpawner : MonoBehaviour
{
    private TurnManager turnManager;

    private int kingCountDown = 1;

    [Header("King Spawn Settings")]
    [SerializeField] private int kingSpawnEveryTurns = 2;
    [SerializeField] private int kingPiecesPerSpawn = 8;
    [SerializeField] private int kingSpawnColumnX = 2;
    [SerializeField] private int kingTopSpawnY = 7;
    [SerializeField] private int kingBottomSpawnY = 0;

    private bool kingHasSpawned;
    private int kingTurnCounter;

    void Start()
    {
        turnManager = ServiceResolver.Resolve<TurnManager>();

        if (turnManager != null)
        {
            Debug.Log("EnemyWaveSpawner subscribed to turn manager.");
            turnManager.OnPhaseChanged += HandlePhase;
        }
        else
        {
            Debug.Log("EnemyWaveSpawner could not find TurnManager.");
        }
    }
    
    private void HandlePhase(TurnPhase phase)
    {
        Debug.Log("EnemyWaveSpawner saw phase: " + phase +
                  " | kingHasSpawned: " + kingHasSpawned + " | kingTurnCounter: " + kingTurnCounter +
                  " | kingCountDown: " + kingCountDown);

        if (phase != TurnPhase.EnemyTurnStart)
            return;

        if (!kingHasSpawned)
        {
            Debug.Log("King has not spawned yet. King countdown: " + kingCountDown);

            if (kingCountDown <= 1 && kingCountDown > 0)
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

            return;
        }

        kingTurnCounter++;

        Debug.Log("King spawn counter: " + kingTurnCounter);

        if (kingTurnCounter % kingSpawnEveryTurns == 0)
        {
            Debug.Log("King is supposed to spawn pieces now.");
            SpawnKingColumnPieces();
        }
        else
        {
            Debug.Log("King is not spawning this turn.");
        }
    }
    
    void SpawnTurnEnemies()
    {
        int spawnCount = 2;

        Debug.Log("Spawning normal turn enemies. Spawn count: " + spawnCount);

        for (int i = 0; i < spawnCount; i++)
        {
            SpawnRandomPiece();
            kingCountDown++;
        }

        Debug.Log("King countdown after normal spawns: " + kingCountDown);
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

    private void SpawnRandomPieceAt(GridPosition pos)
    {
        int i = Random.Range(0, 5);

        if (i == 0)
        {
            Debug.Log("King spawned pawn at: " + pos);
            BoardManager.main.SpawnEnemyPawn(pos);
        }
        else if (i == 1)
        {
            Debug.Log("King spawned knight at: " + pos);
            BoardManager.main.SpawnEnemyKnight(pos);
        }
        else if (i == 2)
        {
            Debug.Log("King spawned bishop at: " + pos);
            BoardManager.main.SpawnEnemyBishop(pos);
        }
        else if (i == 3)
        {
            Debug.Log("King spawned rook at: " + pos);
            BoardManager.main.SpawnEnemyRook(pos);
        }
        else if (i == 4)
        {
            Debug.Log("King spawned queen at: " + pos);
            BoardManager.main.SpawnEnemyQueen(pos);
        }
    }

    private void SpawnPawnInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning pawn in lane: " + pos);
            BoardManager.main.SpawnEnemyPawn(pos);
        }
        else
        {
            Debug.Log("Could not spawn pawn. Tile occupied: " + pos);
        }
    }
    
    private void SpawnKnightInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning knight in lane: " + pos);
            BoardManager.main.SpawnEnemyKnight(pos);
        }
        else
        {
            Debug.Log("Could not spawn knight. Tile occupied: " + pos);
        }
    }
    
    private void SpawnBishopInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning bishop in lane: " + pos);
            BoardManager.main.SpawnEnemyBishop(pos);
        }
        else
        {
            Debug.Log("Could not spawn bishop. Tile occupied: " + pos);
        }
    }
    
    private void SpawnRookInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning rook in lane: " + pos);
            BoardManager.main.SpawnEnemyRook(pos);
        }
        else
        {
            Debug.Log("Could not spawn rook. Tile occupied: " + pos);
        }
    }
    
    private void SpawnQueenInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning queen in lane: " + pos);
            BoardManager.main.SpawnEnemyQueen(pos);
        }
        else
        {
            Debug.Log("Could not spawn queen. Tile occupied: " + pos);
        }
    }
    
    private void SpawnKingIn()
    {
        var pos = new GridPosition(0, 4);

        if (!BoardManager.main.Board.GetTile(pos).IsOccupied)
        {
            Debug.Log("Spawning KING in lane: " + pos);
            BoardManager.main.SpawnEnemyKing(pos);

            kingHasSpawned = true;
            kingTurnCounter = 0;

            Debug.Log("King has spawned. kingHasSpawned set to true.");
        }
        else
        {
            Debug.Log("Could not spawn KING. Tile occupied: " + pos);
        }
    }

    private void SpawnKingColumnPieces()
    {
        int spawned = 0;

        Debug.Log("King spawn attempt started. Column X: " + kingSpawnColumnX);

        for (int y = kingTopSpawnY; y >= kingBottomSpawnY; y--)
        {
            if (spawned >= kingPiecesPerSpawn)
            {
                Debug.Log("King finished spawning. Spawned: " + spawned);
                return;
            }

            var pos = new GridPosition(kingSpawnColumnX, y);
            var tile = BoardManager.main.Board.GetTile(pos);

            if (tile == null)
            {
                Debug.Log("No tile found at: " + kingSpawnColumnX + "," + y);
                continue;
            }

            if (tile.IsOccupied)
            {
                Debug.Log("King spawn tile occupied at: " + kingSpawnColumnX + "," + y);
                continue;
            }

            Debug.Log("King spawning piece at: " + kingSpawnColumnX + "," + y);
            SpawnRandomPieceAt(pos);
            spawned++;
        }

        Debug.Log("King spawn attempt ended. Spawned: " + spawned);
    }
    
    private void OnDestroy()
    {
        if (turnManager != null)
        {
            Debug.Log("EnemyWaveSpawner unsubscribed from turn manager.");
            turnManager.OnPhaseChanged -= HandlePhase;
        }
    }
}