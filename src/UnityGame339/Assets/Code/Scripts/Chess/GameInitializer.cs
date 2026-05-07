using Game.Runtime;
using Game339.Shared.Services.Implementation;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
        var turnManager = ServiceResolver.Resolve<TurnManager>();
        turnManager.StartGame();
    }
}