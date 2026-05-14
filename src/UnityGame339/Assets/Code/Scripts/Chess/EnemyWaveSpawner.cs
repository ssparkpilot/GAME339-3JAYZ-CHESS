using System.Collections;
using Game.Runtime;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;

public class EnemyWaveSpawner : MonoBehaviour
{
    public static EnemyWaveSpawner main;

    private TurnManager turnManager;

    [Header("Enemy Spawn Settings")]
    [SerializeField] private int enemiesPerTurn = 7;
    [SerializeField] private int enemiesDefeatedToSpawnKing = 18;

    [Header("King Position")]
    [SerializeField] private int kingSpawnX = 0;
    [SerializeField] private int kingSpawnY = 4;

    [Header("King Spawn Settings")]
    [SerializeField] private int kingSpawnEveryTurns = 2;
    [SerializeField] private int kingPiecesPerSpawn = 8;
    [SerializeField] private int kingSpawnColumnX = 2;
    [SerializeField] private int kingTopSpawnY = 7;
    [SerializeField] private int kingBottomSpawnY = 0;

    private bool kingHasSpawned;
    private int kingTurnCounter;
    private int enemiesDefeated;

    private void Awake()
    {
        main = this;
    }

    private void Start()
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

    public void RegisterEnemyDefeated(GameObject defeatedEnemy)
    {
        if (kingHasSpawned)
            return;

        if (defeatedEnemy.GetComponent<KingDeathWatcher>() != null)
            return;

        enemiesDefeated++;

        Debug.Log("Enemy defeated count: " + enemiesDefeated + "/" + enemiesDefeatedToSpawnKing);
    }

    private void HandlePhase(TurnPhase phase)
    {
        Debug.Log("EnemyWaveSpawner saw phase: " + phase +
                  " | kingHasSpawned: " + kingHasSpawned +
                  " | kingTurnCounter: " + kingTurnCounter +
                  " | enemiesDefeated: " + enemiesDefeated);

        if (phase != TurnPhase.EnemyTurnStart)
            return;

        if (!kingHasSpawned)
        {
            if (enemiesDefeated >= enemiesDefeatedToSpawnKing)
            {
                bool spawnedKing = SpawnKingIn();

                if (spawnedKing)
                {
                    return;
                }

                Debug.Log("King failed to spawn this turn. Normal enemies will keep spawning.");
                SpawnTurnEnemies();
                return;
            }

            SpawnTurnEnemies();
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

    private void SpawnTurnEnemies()
    {
        int spawned = 0;
        int attempts = 0;
        int maxAttempts = enemiesPerTurn * 20;

        Debug.Log("Spawning normal turn enemies. Target spawn count: " + enemiesPerTurn);

        while (spawned < enemiesPerTurn && attempts < maxAttempts)
        {
            if (SpawnRandomPiece())
            {
                spawned++;
            }

            attempts++;
        }

        Debug.Log("Finished spawning enemies. Actually spawned: " + spawned);
    }

    private bool SpawnRandomPiece()
    {
        int i = Random.Range(0, 5);

        if (i == 0)
            return SpawnPawnInRandomLane();
        else if (i == 1)
            return SpawnKnightInRandomLane();
        else if (i == 2)
            return SpawnBishopInRandomLane();
        else if (i == 3)
            return SpawnRookInRandomLane();
        else if (i == 4)
            return SpawnQueenInRandomLane();

        return false;
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

    private bool IsReservedKingTile(GridPosition pos)
    {
        return !kingHasSpawned && pos.X == kingSpawnX && pos.Y == kingSpawnY;
    }

    private bool CanSpawnAt(GridPosition pos)
    {
        var tile = BoardManager.main.Board.GetTile(pos);

        if (tile == null)
            return false;

        if (tile.IsOccupied)
            return false;

        if (IsReservedKingTile(pos))
            return false;

        return true;
    }

    private bool SpawnPawnInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (CanSpawnAt(pos))
        {
            Debug.Log("Spawning pawn in lane: " + pos);
            BoardManager.main.SpawnEnemyPawn(pos);
            return true;
        }

        Debug.Log("Could not spawn pawn. Tile occupied/reserved: " + pos);
        return false;
    }

    private bool SpawnKnightInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (CanSpawnAt(pos))
        {
            Debug.Log("Spawning knight in lane: " + pos);
            BoardManager.main.SpawnEnemyKnight(pos);
            return true;
        }

        Debug.Log("Could not spawn knight. Tile occupied/reserved: " + pos);
        return false;
    }

    private bool SpawnBishopInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (CanSpawnAt(pos))
        {
            Debug.Log("Spawning bishop in lane: " + pos);
            BoardManager.main.SpawnEnemyBishop(pos);
            return true;
        }

        Debug.Log("Could not spawn bishop. Tile occupied/reserved: " + pos);
        return false;
    }

    private bool SpawnRookInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (CanSpawnAt(pos))
        {
            Debug.Log("Spawning rook in lane: " + pos);
            BoardManager.main.SpawnEnemyRook(pos);
            return true;
        }

        Debug.Log("Could not spawn rook. Tile occupied/reserved: " + pos);
        return false;
    }

    private bool SpawnQueenInRandomLane()
    {
        int y = Random.Range(0, 8);
        var pos = new GridPosition(0, y);

        if (CanSpawnAt(pos))
        {
            Debug.Log("Spawning queen in lane: " + pos);
            BoardManager.main.SpawnEnemyQueen(pos);
            return true;
        }

        Debug.Log("Could not spawn queen. Tile occupied/reserved: " + pos);
        return false;
    }

    private bool SpawnKingIn()
    {
        var pos = new GridPosition(kingSpawnX, kingSpawnY);
        var tile = BoardManager.main.Board.GetTile(pos);

        if (tile == null)
        {
            Debug.Log("Could not spawn KING. No tile at: " + pos);
            return false;
        }

        if (!tile.IsOccupied)
        {
            Debug.Log("Spawning KING in lane: " + pos);
            BoardManager.main.SpawnEnemyKing(pos);

            kingHasSpawned = true;
            kingTurnCounter = 0;

            Debug.Log("King has spawned. kingHasSpawned set to true.");
            return true;
        }

        Debug.Log("Could not spawn KING. Tile occupied: " + pos);
        return false;
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