using UnityEngine;
using Game339.Shared.Models;
using System.Collections;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;

    private EnemyUnit unit;
    private Coroutine moveRoutine;

    public void Init(EnemyUnit enemyUnit)
    {
        unit = enemyUnit;

        // Snap instantly on spawn
        ChessPlot plot = BoardManager.main.GetPlot(unit.Position);
        if (plot != null)
            transform.position = plot.transform.position;
    }

    public void UpdatePosition()
    {
        if (unit == null)
            return;

        ChessPlot targetPlot = BoardManager.main.GetPlot(unit.Position);
        if (targetPlot == null)
            return;

        // Stop previous movement if still running
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveTo(targetPlot.transform.position));
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = transform.position;
        float distance = Vector3.Distance(start, target);
        float t = 0f;

        if (distance < 0.001f)
        {
            transform.position = target;
            yield break;
        }

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed / distance;
            float eased = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(start, target, eased);
            yield return null;
        }

        transform.position = target;
    }

    private void OnMouseEnter()
    {
        if (HoverHealthUI.main == null) return;
        if (unit == null) return;

        HoverHealthUI.main.Show(transform.position, unit.Health);
    }

    private void OnMouseExit()
    {
        if (HoverHealthUI.main != null)
        {
            HoverHealthUI.main.Hide();
        }
    }
}