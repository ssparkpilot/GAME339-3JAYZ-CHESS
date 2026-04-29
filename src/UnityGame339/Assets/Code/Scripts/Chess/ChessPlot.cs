using UnityEngine;
using Game339.Shared.Models;

public class ChessPlot : MonoBehaviour
{
    [Header("Grid Position")]
    [SerializeField] private int gridX;
    [SerializeField] private int gridY;

    public GridPosition GridPos => new GridPosition(gridX, gridY);

    private void OnMouseDown()
    {
        Debug.Log($"Clicked chess plot at {gridX},{gridY}");
    }

    public bool TryGetUnit(out Unit unit)
    {
        unit = null;
        var tile = BoardManager.main.GetTile(GridPos);
        if (tile?.Occupant != null)
        {
            unit = tile.Occupant;
            return true;
        }
        return false;
    }
    
    private void OnValidate()
    {
        if (!transform.parent) return;

        // Example: grid based on local position
        gridX = Mathf.RoundToInt(transform.localPosition.x);
        gridY = Mathf.RoundToInt(transform.localPosition.y);
    }
    
    public bool TryGetTower(out TowerUnit tower)
    {
        tower = null;

        var tile = BoardManager.main.GetTile(GridPos);
        if (tile?.Occupant is TowerUnit t)
        {
            tower = t;
            return true;
        }

        return false;
    }

    public bool TryGetEnemy(out EnemyUnit enemy)
    {
        enemy = null;

        var tile = BoardManager.main.GetTile(GridPos);
        if (tile?.Occupant is EnemyUnit e)
        {
            enemy = e;
            return true;
        }

        return false;
    }

    private void OnMouseEnter()
    {
        if (TryGetEnemy(out EnemyUnit enemy))
        {
            HoverHealthUI.main.Show(transform.position, enemy.Health);
        }
    }

    private void OnMouseExit()
    {
        HoverHealthUI.main.Hide();
    }
}