using System.Collections.Generic;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;

public class BoardManager : MonoBehaviour
{
    public static BoardManager main;

    // --- Core Board ---
    public GridBoard Board { get; private set; }

    // --- Plot mapping (Unity view <-> grid data) ---
    private readonly Dictionary<GridPosition, Plot> plotLookup =
        new Dictionary<GridPosition, Plot>();

    private void Awake()
    {
        if (main != null)
        {
            Destroy(gameObject);
            return;
        }

        main = this;
        Board = new GridBoard();

        RegisterPlots();
    }

    // -------------------------
    // Plot Registration
    // -------------------------
    private void RegisterPlots()
    {
        Plot[] plots = FindObjectsOfType<Plot>();

        foreach (var plot in plots)
        {
            GridPosition pos = plot.GridPos;

            if (plotLookup.ContainsKey(pos))
            {
                Debug.LogError(
                    $"Duplicate plot at {pos.X},{pos.Y}");
                continue;
            }

            plotLookup[pos] = plot;
        }
    }

    // -------------------------
    // Unit Placement
    // -------------------------
    public void PlaceUnit(Unit unit, GridPosition position)
    {
        var tile = Board.GetTile(position);
        if (tile == null || tile.IsOccupied)
            return;

        tile.Place(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        var tile = Board.GetTile(unit.Position);
        if (tile == null)
            return;

        tile.Clear();
    }

    public void MoveUnit(Unit unit, GridPosition to)
    {
        Board.MoveUnit(unit, to);
    }

    // -------------------------
    // Queries
    // -------------------------
    public GridTile GetTile(GridPosition position)
    {
        return Board.GetTile(position);
    }

    public Plot GetPlot(GridPosition position)
    {
        plotLookup.TryGetValue(position, out Plot plot);
        return plot;
    }

    public IEnumerable<EnemyUnit> GetAllEnemies()
    {
        foreach (var tile in Board.GetAllTiles())
        {
            if (tile.Occupant is EnemyUnit enemy)
                yield return enemy;
        }
    }
}
