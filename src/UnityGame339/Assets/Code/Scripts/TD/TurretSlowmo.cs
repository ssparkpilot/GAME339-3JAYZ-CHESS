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
    [Header("Attribute")]
    [SerializeField] private int bellDamage = 5;
    
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
            Health health = hit.transform.gameObject.GetComponent<Health>();
            //EnemyView em = hit.transform.GetComponent<EnemyView>();
            if (health == null)
                continue;

            // damage here
            health.TakeDamage(bellDamage);
        }
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
