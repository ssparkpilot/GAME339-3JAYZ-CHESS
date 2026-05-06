using System.Collections.Generic;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;

public class BoardManager : MonoBehaviour
{
    public static BoardManager main;

    [Header("Prefabs")]
    [SerializeField] private GameObject pawnPrefab;
    [SerializeField] private GameObject knightPrefab;
    [SerializeField] private GameObject bishopPrefab;

    public GridBoard Board { get; private set; }

    private readonly Dictionary<GridPosition, ChessPlot> plotLookup =
        new Dictionary<GridPosition, ChessPlot>();
    
    public EnemyUnit DebugPawn { get; private set; }

    private void Awake()
    {
        main = this;
        Board = new GridBoard();
        RegisterPlots();
    }

    private void RegisterPlots()
    {
        ChessPlot[] plots = FindObjectsOfType<ChessPlot>();

        foreach (var plot in plots)
        {
            plotLookup[plot.GridPos] = plot;
        }
    }

    public GridTile GetTile(GridPosition pos)
    {
        return Board.GetTile(pos);
    }

    public ChessPlot GetPlot(GridPosition pos)
    {
        plotLookup.TryGetValue(pos, out ChessPlot plot);
        return plot;
    }

    public EnemyUnit SpawnEnemyPawn(GridPosition pos)
    {
        var enemyUnit = new EnemyUnit(
            pos,
            new PawnMovementRule(),
            health: 20
        );

        Board.GetTile(pos).Place(enemyUnit);

        ChessPlot plot = GetPlot(pos);
        GameObject enemyGO = Instantiate(
            pawnPrefab,
            plot.transform.position,
            Quaternion.identity
        );

        enemyGO.GetComponent<EnemyView>().Init(enemyUnit);

        return enemyUnit;
    }
    
    public EnemyUnit SpawnEnemyKnight(GridPosition pos)
    {
        var enemyUnit = new EnemyUnit(
            pos,
            new KnightMovementRule(),
            health: 20
        );

        Board.GetTile(pos).Place(enemyUnit);

        ChessPlot plot = GetPlot(pos);
        GameObject enemyGO = Instantiate(
            knightPrefab,
            plot.transform.position,
            Quaternion.identity
        );

        enemyGO.GetComponent<EnemyView>().Init(enemyUnit);

        return enemyUnit;
    }
    
    public EnemyUnit SpawnEnemyBishop(GridPosition pos)
    {
        var enemyUnit = new EnemyUnit(
            pos,
            new BishopMovementRule(),
            health: 20
        );

        Board.GetTile(pos).Place(enemyUnit);

        ChessPlot plot = GetPlot(pos);
        GameObject enemyGO = Instantiate(
            bishopPrefab,
            plot.transform.position,
            Quaternion.identity
        );

        enemyGO.GetComponent<EnemyView>().Init(enemyUnit);

        return enemyUnit;
    }
}