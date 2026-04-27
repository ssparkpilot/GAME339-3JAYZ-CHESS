using UnityEngine;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;

public class TurnManagerBehaviour : MonoBehaviour
{
    public static TurnManagerBehaviour main;

    public ITurnManager TurnManager { get; private set; }

    private void Awake()
    {
        main = this;
        TurnManager = new TurnManager();
        TurnManager.StartGame();
    }

    public void EndPhaseButton()
    {
        TurnManager.AdvancePhase();
    }
}