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
    [SerializeField] private float sniperBps = 1f; // bullets per second
    [SerializeField] private float sniperAps = 4f; // attacks per second
    [SerializeField] private float sniperMps = 4f; // money per second
    [SerializeField] private int sniperBaseUpgradeCost = 100;
    [SerializeField] private float sniperTargetingRangeBase;

    public int sniperTowerIndex;

    private float sniperBpsBase;
    private float sniperApsBase;
    private float sniperMpsBase;

    private Transform sniperTarget;
    public float sniperTimeUntilFire;

    private int sniperLevel = 1;

    public AudioSource sniperAudioSource;
    public AudioClip sniperPlaceSound;

    public float sniperMinPitch = 0.8f;
    public float sniperMaxPitch = 1.2f;

    private void Start()
    {
        sniperBpsBase = sniperBps;
        sniperApsBase = sniperAps;
        sniperMpsBase = sniperMps;

        sniperTargetingRangeBase = sniperTargetingRange;

        if (sniperUpgradeButton != null)
        {
            sniperUpgradeButton.onClick.AddListener(Upgrade);
        }
    }

    private void Update()
    {
        if (LevelManager.main.isGameOver)
        {
            return;
        }

        if (sniperTarget == null)
        {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            sniperTarget = null;
        }
        else
        {
            sniperTimeUntilFire += Time.deltaTime;

            if (sniperTimeUntilFire >= 1f / sniperBps)
            {
                Shoot();
                sniperTimeUntilFire = 0f;
            }
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
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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

        if (hits.Length > 0)
        {
            sniperTarget = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        if (sniperTarget == null)
        {
            return false;
        }

        return Vector2.Distance(sniperTarget.position, transform.position) <= sniperTargetingRange;
    }

    public void OpenUpgradeUI()
    {
        if (sniperUpgradeUI != null)
        {
            sniperUpgradeUI.SetActive(true);
        }
    }

    public void CloseUpgradeUI()
    {
        if (sniperUpgradeUI != null)
        {
            sniperUpgradeUI.SetActive(false);
        }

        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
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
}