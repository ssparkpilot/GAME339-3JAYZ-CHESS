using System.Linq;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services;

public class PlayerAttackController : MonoBehaviour
{
    private TowerUnit selectedTower;

    private readonly AttackRangeService rangeService = new();
    private readonly CombatService combatService = new();

    private void Update()
    {
        var turnManager = TurnManagerBehaviour.main.TurnManager;
        if (!turnManager.CanPlayerAct())
            return;

        HandleTowerSelection();
        HandleEnemySelection();
    }

    private void HandleTowerSelection()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        ChessPlot plot = GetPlotUnderMouse();
        if (plot == null)
            return;

        if (plot.TryGetTower(out TowerUnit tower) && !tower.HasActed)
        {
            selectedTower = tower;
            Debug.Log("Tower selected");
        }
    }

    private void HandleEnemySelection()
    {
        if (selectedTower == null || selectedTower.HasActed)
            return;

        if (!Input.GetMouseButtonDown(1))
            return;

        ChessPlot plot = GetPlotUnderMouse();
        if (plot == null)
            return;

        if (!plot.TryGetEnemy(out EnemyUnit enemy))
            return;

        bool inRange = rangeService
            .GetEnemiesInRange(selectedTower, BoardManager.main.Board)
            .Contains(enemy);

        if (!inRange)
            return;

        combatService.ResolveAttack(selectedTower, enemy);
        selectedTower.MarkUsed();
        selectedTower = null;
    }

    private ChessPlot GetPlotUnderMouse()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hit))
            return hit.collider.GetComponent<ChessPlot>();

        return null;
    }
}