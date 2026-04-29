using UnityEngine;
using Game339.Shared.Models;

public class EnemyView : MonoBehaviour
{
    private EnemyUnit unit;

    public void Init(EnemyUnit enemyUnit)
    {
        unit = enemyUnit;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        if (unit == null)
            return;

        ChessPlot plot = BoardManager.main.GetPlot(unit.Position);
        if (plot == null)
            return;

        transform.position = plot.transform.position;
    }
}