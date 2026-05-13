using Game.Runtime;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;
using UnityEngine;

public class SummonTower : Turret
{
    [Header("Attribute")]
    //[SerializeField] private int currencyWorth = 10;
    [SerializeField] private int cooldownTurns=1;

    public GridPosition GridPos;
    public int type;

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
        
        if(cooldownTurns > 0)
        {
            cooldownTurns--;
            return;
        }
        //creates variable pos which is the grid position of the tower
        
        int spawny = (int)(transform.position.y+6);
        GridPosition pos = GridPos;
        Debug.Log("CHESS at position: " + GridPos.X + ", " + GridPos.Y);
        if (pos.X < 1)
        {
            Debug.Log("Invalid grid position for SummonTower: " + pos);
            return;
        }
        else
        {
            if (type == 0)
            {
                BoardManager.main.SpawnChud(pos);

            }
            else
            {
                BoardManager.main.SpawnChudTank(pos);

            }
            cooldownTurns = 1; // reset cooldown after spawning

            

        }

    }
    
    private new void OnDestroy()
    {
        if (turnManager != null)
            turnManager.OnTurnStateChanged -= HandleTurnChanged;
    }
}
