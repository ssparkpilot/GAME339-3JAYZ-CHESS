using UnityEngine;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using System.Linq;
using Game339.Shared.Diagnostics;


public class TurnManagerBehaviour : MonoBehaviour
{
    public static TurnManagerBehaviour main;
    private readonly IGameLog _log;


    public ITurnManager TurnManager { get; private set; }

    private void Awake()
    {
        main = this;
        TurnManager = new TurnManager(_log);
        TurnManager.StartGame();
    }

    public void EndPhaseButton()
    {
        TurnManager.AdvancePhase();
    }
}