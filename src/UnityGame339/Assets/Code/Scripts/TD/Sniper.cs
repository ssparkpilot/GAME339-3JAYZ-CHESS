using Game.Runtime;
using Game339.Shared.Models;
using Game339.Shared.Services.Implementation;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Sniper : Turret
{
    [Header("Sniper References")]
    [SerializeField] private LayerMask sniperEnemyMask;
    [SerializeField] private GameObject sniperBulletPrefab;
    [SerializeField] private Transform sniperFiringPoint;
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private GameObject sniperUpgradeUI;
    [SerializeField] private Button sniperUpgradeButton;

    [Header("Sniper Attributes")]
    [SerializeField] private float sniperTargetingRange = 3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float sniperBps = 1f;
    [SerializeField] private float sniperAps = 4f;
    [SerializeField] private float sniperMps = 4f;
    [SerializeField] private int sniperBaseUpgradeCost = 100;
    [SerializeField] private float sniperTargetingRangeBase;

    public int sniperTowerIndex;

    private float sniperBpsBase;
    private float sniperApsBase;
    private float sniperMpsBase;

    private Transform sniperTarget;

    private int sniperLevel = 1;

    public AudioSource sniperAudioSource;
    public AudioClip sniperPlaceSound;

    public float sniperMinPitch = 0.8f;
    public float sniperMaxPitch = 1.2f;

    private TurnManager turnManager;
    private bool hasFiredThisTurn;

    private new void Start()
    {
        sniperBpsBase = sniperBps;
        sniperApsBase = sniperAps;
        sniperMpsBase = sniperMps;

        sniperTargetingRangeBase = sniperTargetingRange;

        if (sniperUpgradeButton != null)
        {
            sniperUpgradeButton.onClick.AddListener(Upgrade);
        }

        turnManager = ServiceResolver.Resolve<TurnManager>();

        if (turnManager != null)
        {
            turnManager.OnTurnStateChanged += HandleTurnChanged;
        }
    }

    private void Update()
    {
        if (LevelManager.main != null && LevelManager.main.isGameOver)
        {
            return;
        }

        if (sniperTarget != null && !CheckTargetIsInRange())
        {
            sniperTarget = null;
        }

        if (sniperTarget != null)
        {
            RotateTowardsTarget();
        }
    }

    private void HandleTurnChanged(TurnOwner owner, TurnPhase phase)
    {
        if (owner == TurnOwner.Player && phase == TurnPhase.PlayerTurnStart)
        {
            hasFiredThisTurn = false;
        }

        if (!hasFiredThisTurn && owner == TurnOwner.Player && phase == TurnPhase.PlayerActing)
        {
            TryFireOnce();
            hasFiredThisTurn = true;
        }
    }

    private void TryFireOnce()
    {
        if (!this || !gameObject.activeInHierarchy)
            return;

        FindTarget();

        if (sniperTarget != null && CheckTargetIsInRange())
        {
            RotateTowardsTarget();
            Shoot();
        }
    }

    private void RotateTowardsTarget()
    {
        if (sniperTarget == null || turretRotationPoint == null)
        {
            return;
        }

        float angle = Mathf.Atan2(
            sniperTarget.position.y - turretRotationPoint.position.y,
            sniperTarget.position.x - turretRotationPoint.position.x
        ) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void Shoot()
    {
        if (sniperBulletPrefab == null || sniperFiringPoint == null || sniperTarget == null)
        {
            return;
        }

        GameObject bulletObj = Instantiate(sniperBulletPrefab, sniperFiringPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetTarget(sniperTarget);
        }

        if (sniperAudioSource != null && sniperPlaceSound != null)
        {
            sniperAudioSource.pitch = Random.Range(sniperMinPitch, sniperMaxPitch);
            //make the audiosource play at half the volume
            sniperAudioSource.volume = 0.25f;
            //play the place sound at the randomized pitch
            sniperAudioSource.PlayOneShot(sniperPlaceSound);
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position,
            sniperTargetingRange,
            Vector2.zero,
            0f,
            sniperEnemyMask
        );

        if (hits.Length == 0)
        {
            sniperTarget = null;
            return;
        }

        ChessPlot sniperPlot = FindClosestPlot(transform.position);

        Transform bestSameLaneTarget = null;
        int bestSameLaneHP = -1;
        float bestSameLaneDistance = float.MaxValue;

        Transform bestAnyLaneTarget = null;
        int bestAnyLaneHP = -1;
        float bestAnyLaneDistance = float.MaxValue;

        foreach (RaycastHit2D hit in hits)
        {
            EnemyView enemyView = hit.transform.GetComponentInParent<EnemyView>();
            Health enemyHealth = hit.transform.GetComponentInParent<Health>();
            KingDeathWatcher king = hit.transform.GetComponentInParent<KingDeathWatcher>();

            if (king != null)
                continue;

            if (enemyView == null || enemyView.Unit == null)
                continue;

            if (enemyHealth == null)
                continue;

            Transform enemyTransform = enemyView.transform;
            int enemyHP = enemyHealth.CurrentHP;
            float distance = Vector2.Distance(transform.position, enemyTransform.position);

            if (enemyHP > bestAnyLaneHP || enemyHP == bestAnyLaneHP && distance < bestAnyLaneDistance)
            {
                bestAnyLaneHP = enemyHP;
                bestAnyLaneDistance = distance;
                bestAnyLaneTarget = enemyTransform;
            }

            if (sniperPlot == null)
                continue;

            if (enemyView.Unit.Position.Y == sniperPlot.GridPos.Y)
            {
                if (enemyHP > bestSameLaneHP || enemyHP == bestSameLaneHP && distance < bestSameLaneDistance)
                {
                    bestSameLaneHP = enemyHP;
                    bestSameLaneDistance = distance;
                    bestSameLaneTarget = enemyTransform;
                }
            }
        }

        if (bestSameLaneTarget != null)
        {
            sniperTarget = bestSameLaneTarget;
        }
        else
        {
            sniperTarget = bestAnyLaneTarget;
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
        if (sniperTarget == null)
        {
            return false;
        }

        return Vector2.Distance(sniperTarget.position, transform.position) <= sniperTargetingRange;
    }

    public new void OpenUpgradeUI()
    {
        if (sniperUpgradeUI != null)
        {
            sniperUpgradeUI.SetActive(true);
        }
    }

    public new void CloseUpgradeUI()
    {
        if (sniperUpgradeUI != null)
        {
            sniperUpgradeUI.SetActive(false);
        }

        if (UIManager.main != null)
        {
            UIManager.main.SetHoveringState(false);
        }
    }

    public new void Upgrade()
    {
        if (LevelManager.main == null)
        {
            return;
        }

        if (CalculateCost() > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(CalculateCost());

        sniperLevel++;

        sniperBps = CalculateBPS();
        sniperAps = CalculateAPS();
        sniperMps = CalculateMPS();
        sniperTargetingRange = CalculateRange();

        CloseUpgradeUI();

        Debug.Log("New sniper level: " + sniperLevel);
        Debug.Log("New sniper BPS: " + sniperBps);
        Debug.Log("New sniper targeting range: " + sniperTargetingRange);
        Debug.Log("New sniper cost: " + CalculateCost());
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(sniperBaseUpgradeCost * Mathf.Pow(sniperLevel, 0.8f));
    }

    private float CalculateBPS()
    {
        return sniperBpsBase * Mathf.Pow(sniperLevel, 0.6f);
    }

    private float CalculateAPS()
    {
        return sniperApsBase * Mathf.Pow(sniperLevel, 0.6f);
    }

    private float CalculateMPS()
    {
        return sniperMpsBase * Mathf.Pow(sniperLevel, 0.6f);
    }

    private float CalculateRange()
    {
        return sniperTargetingRangeBase * Mathf.Pow(sniperLevel, 0.4f);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, sniperTargetingRange);
    }

    protected override void OnDestroy()
    {
        if (turnManager != null)
        {
            turnManager.OnTurnStateChanged -= HandleTurnChanged;
        }
    }
}