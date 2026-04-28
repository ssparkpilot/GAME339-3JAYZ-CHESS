using UnityEngine;
using Game339.Shared.Models;

public class EnemyView : MonoBehaviour
{
    public EnemyUnit Unit { get; private set; }

    public void Init(EnemyUnit unit)
    {
        Unit = unit;
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        ChessPlot plot = BoardManager.main.GetPlot(Unit.Position);
        if (plot != null)
            transform.position = plot.transform.position;
    }
}