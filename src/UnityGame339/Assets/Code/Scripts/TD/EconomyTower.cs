using Game.Runtime;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;
using UnityEngine;

public class EconomyTower : Turret
{
    [Header("Attribute")]
    [SerializeField] private int currencyWorth = 10;
    
    public GameObject FloatingScorePrefab;
    
    private TurnManager turnManager;
    private bool hasGeneratedThisTurn;
    
    protected new void Start()
    {
        base.Start(); // important because it inherits from Turret

        turnManager = ServiceResolver.Resolve<TurnManager>();
        turnManager.OnTurnStateChanged += HandleTurnChanged;
    }
    
    private void HandleTurnChanged(TurnOwner owner, TurnPhase phase)
    {
        if (this == null) return;
        
        // Reset at start of player turn
        if (owner == TurnOwner.Player && phase == TurnPhase.PlayerTurnStart)
        {
            hasGeneratedThisTurn = false;
        }

        // Generate money once when player turn starts
        if (!hasGeneratedThisTurn && owner == TurnOwner.Player && phase == TurnPhase.PlayerTurnStart)
        {
            GenerateOnce();
            hasGeneratedThisTurn = true;
        }
    }
    
    private void GenerateOnce()
    {
        // scale money with mps (for upgrades that is not active at this time)
        int amount = Mathf.RoundToInt(currencyWorth * mps);

        LevelManager.main.IncreaseCurrency(amount);

        CreateDeathEffect();

        var obj = Instantiate(FloatingScorePrefab, transform.position, Quaternion.identity);
        var floatingText = obj.GetComponent<FloatingText>();
        floatingText.SetText(amount);
    }
    
    private void OnDestroy()
    {
        if (turnManager != null)
            turnManager.OnTurnStateChanged -= HandleTurnChanged;
    }
}
