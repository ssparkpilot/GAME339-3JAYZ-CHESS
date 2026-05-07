using System.Collections;
using Game.Runtime;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TurretSlowmo : Turret
{
    [Header("References")]
    
    [Header("Attribute")]
    [SerializeField] private float freezeTime = 1f;
    
    public AudioClip fireSound;
    
    private TurnManager turnManager;
    private bool hasActivatedThisTurn;
    
    protected new void Start()
    {
        base.Start();

        turnManager = ServiceResolver.Resolve<TurnManager>();
        turnManager.OnTurnStateChanged += HandleTurnChanged;
    }
    
    private void ActivateSlowmo()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            targetingRange,
            Vector2.zero,
            0f,
            enemyMask
        );

        if (hits.Length == 0)
            return;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = 0.75f;
        audioSource.PlayOneShot(fireSound);

        CreateDeathEffect();

        foreach (var hit in hits)
        {
            EnemyView em = hit.transform.GetComponent<EnemyView>();
            if (em == null)
                continue;

            em.UpdateSpeed(0.5f);
            em.FreezeTint();

            StartCoroutine(ResetEnemySpeedTurnBased(em, 1)); // freeze for 1 turn
        }
    }

    private IEnumerator ResetEnemySpeedTurnBased(EnemyView em, int turns)
    {
        int startTurn = turnManager.GetTurnNumber();

        while (turnManager.GetTurnNumber() < startTurn + turns)
        {
            yield return null;
        }

        if (em != null)
            em.ResetSpeed();
    }
    
    private void OnDrawGizmosSelected(){
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
    
    private void HandleTurnChanged(TurnOwner owner, TurnPhase phase)
    {
        // reset at start of player turn
        if (owner == TurnOwner.Player && phase == TurnPhase.PlayerTurnStart)
        {
            hasActivatedThisTurn = false;
        }

        // activate once when enemy turn starts
        if (!hasActivatedThisTurn && owner == TurnOwner.Enemy && phase == TurnPhase.EnemyTurnStart)
        {
            ActivateSlowmo();
            hasActivatedThisTurn = true;
        }
    }
    
    private void OnDestroy()
    {
        if (turnManager != null)
            turnManager.OnTurnStateChanged -= HandleTurnChanged;
    }
}
