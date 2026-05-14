using UnityEngine;

public class KingDeathWatcher : MonoBehaviour
{
    private Health health;
    private bool hasTriggeredWin;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void Update()
    {
        if (hasTriggeredWin)
            return;

        if (health == null)
            return;

        if (health.CurrentHP <= 0)
        {
            TriggerWin();
        }
    }

    private void OnDestroy()
    {
        if (hasTriggeredWin)
            return;

        if (LevelManager.main == null)
            return;

        if (LevelManager.main.IsGameOver)
            return;

        TriggerWin();
    }

    private void TriggerWin()
    {
        if (hasTriggeredWin)
            return;

        hasTriggeredWin = true;

        if (LevelManager.main != null)
        {
            LevelManager.main.KingDefeated();
        }
    }
}