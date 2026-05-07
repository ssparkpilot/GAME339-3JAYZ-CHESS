using Game.Runtime;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;
using UnityEngine;

public class TurnFlowController : MonoBehaviour
{
    private TurnManager turnManager;

    private void Start()
    {
        turnManager = ServiceResolver.Resolve<TurnManager>();
        turnManager.OnTurnStateChanged += HandlePhase;
    }

    private void OnDestroy()
    {
        turnManager.OnTurnStateChanged -= HandlePhase;
    }

    private void HandlePhase(TurnOwner owner, TurnPhase phase)
    {
        if (phase == TurnPhase.PlayerTurnStart)
        {
            turnManager.AdvancePhase(); // -> PlayerActing
        }
        else if (phase == TurnPhase.PlayerTurnEnd)
        {
            turnManager.AdvancePhase(); // -> EnemyTurnStart
        }
        else if (phase == TurnPhase.EnemyTurnStart)
        {
            turnManager.AdvancePhase(); // -> EnemyMoving
        }
        else if (phase == TurnPhase.EnemyTurnEnd)
        {
            turnManager.AdvancePhase(); // -> PlayerTurnStart
        }
    }
}
