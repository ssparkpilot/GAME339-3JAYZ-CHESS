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

    [Header("Attributes")]
    [SerializeField] public float targetingRange = 3f;
    [SerializeField] private float bps = 1f; // bullets per second
    [SerializeField] public float aps = 4f; // attacks per second
    [SerializeField] public float mps = 4f; // money per second
    [SerializeField] private int baseUpgradeCost = 100;
    [SerializeField] private float targetingRangeBase;

    [Header("Health")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    public int CurrentHealth => currentHealth;

    public int towerIndex;
    
    private float bpsBase;
    private float apsBase;
    private float mpsBase;
    
    private Transform target;
    public float timeUntilFire;

    private int level = 1;

    public AudioSource audioSource;
    public AudioClip placeSound;
    
    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Start()
    {
        currentHealth = maxHealth;

        bpsBase = bps;
        apsBase = aps;
        mpsBase = mps;
        
        targetingRangeBase = targetingRange;
        
        upgradeButton.onClick.AddListener(Upgrade);
    }

    private void Update()
    {
        if (LevelManager.main.isGameOver)
        {
            return;
        }
        
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
        
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.volume = 0.25f;
        audioSource.PlayOneShot(placeSound);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Tower HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    private void DestroyTower()
    {
        CreateDeathEffect();
        Destroy(gameObject);
    }


    private void OnMouseEnter()
{
    if (HoverHealthUI.main == null) return;

    Vector3 hoverPosition = transform.position + new Vector3(-0.2f, -0.4f, 0);
    HoverHealthUI.main.Show(hoverPosition, currentHealth);
}

private void OnMouseExit()
{
    if (HoverHealthUI.main != null)
    {
        HoverHealthUI.main.Hide();
    }
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
}