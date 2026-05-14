using System.Collections;
using Game.Runtime;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : DeathEffectObject
{
    [Header("References")]
    [SerializeField] public LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;

    [Header("Attribute")]
    [SerializeField] public float targetingRange = 3f;
    [SerializeField] private float bps = 1f; // bullets per second
    [SerializeField] public float aps = 4f; // attacks per second
    [SerializeField] public float mps = 4f; // money per second
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private float targetingRangeBase;
    [SerializeField] private int bulletsPerShot = 1;

    public int towerIndex;
    
    private float bpsBase;
    private float apsBase;
    private float mpsBase;
    
    private Transform target;

    private int level = 1; // tower upgrade level

    public AudioSource audioSource;
    public AudioClip placeSound;
    
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;
    
    private TurnManager turnManager;
    private bool hasFiredThisTurn;

    public void Start()
    {
        bpsBase = bps;
        apsBase = aps;
        mpsBase = mps;
        
        targetingRangeBase = targetingRange;
        
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(Upgrade);
        }
        
        turnManager = ServiceResolver.Resolve<TurnManager>();
        turnManager.OnTurnStateChanged += HandleTurnChanged;
    }

    private void Update()
    {
        if (LevelManager.main != null && LevelManager.main.isGameOver)
            return;

        if (target != null && !CheckTargetIsInRange())
        {
            target = null;
        }
    }

    private void Shoot()
    {
        if (firingPoint == null) return;

        StartCoroutine(WaitBeforeShooting());
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            targetingRange,
            Vector2.zero,
            0f,
            enemyMask
        );

        if (hits.Length == 0)
        {
            target = null;
            return;
        }

        ChessPlot turretPlot = FindClosestPlot(transform.position);

        Transform closestSameLaneTarget = null;
        float closestSameLaneDistance = float.MaxValue;

        Transform closestAnyLaneTarget = null;
        float closestAnyLaneDistance = float.MaxValue;

        foreach (RaycastHit2D hit in hits)
        {
            Transform enemyTransform = hit.transform;
            float distance = Vector2.Distance(transform.position, enemyTransform.position);

            if (distance < closestAnyLaneDistance)
            {
                closestAnyLaneDistance = distance;
                closestAnyLaneTarget = enemyTransform;
            }

            if (turretPlot == null)
                continue;

            EnemyView enemyView = enemyTransform.GetComponentInParent<EnemyView>();

            if (enemyView == null || enemyView.Unit == null)
                continue;

            if (enemyView.Unit.Position.Y == turretPlot.GridPos.Y)
            {
                if (distance < closestSameLaneDistance)
                {
                    closestSameLaneDistance = distance;
                    closestSameLaneTarget = enemyTransform;
                }
            }
        }

        if (closestSameLaneTarget != null)
        {
            target = closestSameLaneTarget;
        }
        else
        {
            target = closestAnyLaneTarget;
        }
    }

    private ChessPlot FindClosestPlot(Vector3 worldPosition)
    {
        ChessPlot[] plots = Object.FindObjectsByType<ChessPlot>(FindObjectsSortMode.None);

        ChessPlot closestPlot = null;
        float closestDistance = float.MaxValue;

        foreach (ChessPlot plot in plots)
        {
            float distance = Vector2.Distance(worldPosition, plot.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlot = plot;
            }
        }

        return closestPlot;
    }

    private bool CheckTargetIsInRange()
    {
        return target != null && Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (CalculateCost() > LevelManager.main.currency) return;
        
        LevelManager.main.SpendCurrency(CalculateCost());

        level++;

        bps = CalculateBPS();
        aps = CalculateAPS();
        mps = CalculateMPS();
        
        targetingRange = CalculateRange();
        
        CloseUpgradeUI();
        Debug.Log("New level: " + level);
        Debug.Log("New BPS: " + bps);
        Debug.Log("New targeting range: " + targetingRange);
        Debug.Log("New cost: " + CalculateCost());
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBPS()
    {
        return bpsBase * Mathf.Pow(level, 0.6f);
    }
    
    private float CalculateAPS()
    {
        return apsBase * Mathf.Pow(level, 0.6f);
    }
    
    private float CalculateMPS()
    {
        return mpsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
    
    private void HandleTurnChanged(TurnOwner owner, TurnPhase phase)
    {
        // reset at start of player turn
        if (owner == TurnOwner.Player && phase == TurnPhase.PlayerTurnStart)
        {
            hasFiredThisTurn = false;
        }

        // fire once when player turn ends or the enemy turn starts
        if (!hasFiredThisTurn && owner == TurnOwner.Player && phase == TurnPhase.PlayerActing)
        {
            TryFireOnce();
            hasFiredThisTurn = true;
        }
    }

    private IEnumerator WaitBeforeShooting()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 0.75f));

        for (int i = 0; i < bulletsPerShot; i++)
        {
            // If target is invalid, try to get a new one
            if (!target || !CheckTargetIsInRange())
            {
                FindTarget();

                // Still no valid target? Skip this bullet
                if (!target || !CheckTargetIsInRange())
                    continue;
            }

            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            Bullet bulletScript = bulletObj.GetComponent<Bullet>();
            bulletScript.SetTarget(target);

            if (audioSource != null && placeSound != null)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.volume = 0.25f;
                audioSource.PlayOneShot(placeSound);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    
    private void TryFireOnce()
    {
        if (!this || !gameObject.activeInHierarchy)
            return;
        
        FindTarget();

        if (target != null && CheckTargetIsInRange())
        {
            Shoot();
        }
    }
    
    protected virtual void OnDestroy()
    {
        if (turnManager != null)
        {
            turnManager.OnTurnStateChanged -= HandleTurnChanged;
        }
    }
}