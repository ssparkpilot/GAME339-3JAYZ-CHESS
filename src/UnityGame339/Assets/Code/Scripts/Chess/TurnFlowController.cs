using Game.Runtime;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;
using UnityEngine;

public class TurnFlowController : MonoBehaviour
{
    private TurnManager turnManager;

    public AudioSource audioSource;
    public AudioClip yourTurnSound;

    [SerializeField] private Menu menu;

     public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

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
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        //make the audiosource play at half the volume
        //play the place sound at the randomized pitch
            audioSource.PlayOneShot(yourTurnSound);

            turnManager.AdvancePhase(); // -> PlayerActing

            menu.ToggleMenu();
        }
        else if (phase == TurnPhase.PlayerTurnEnd)
        {
            turnManager.AdvancePhase(); // -> EnemyTurnStart
            
            menu.ToggleMenu();
        }
        else if (phase == TurnPhase.EnemyTurnStart)
        {
            //turnManager.AdvancePhase(); // -> EnemyMoving
        }
        else if (phase == TurnPhase.EnemyTurnEnd)
        {
            //turnManager.AdvancePhase(); // -> PlayerTurnStart
        }
    }
}
