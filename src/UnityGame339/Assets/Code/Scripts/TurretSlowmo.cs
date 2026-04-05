using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TurretSlowmo : DeathEffectObject
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    
    [Header("Attribute")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float aps = 4f; // attacks per second
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private int baseUpgradeCost = 100;
    
    private float apsBase;
    private float targetingRangeBase;
    
    private int level = 1; // tower upgrade level
    
    private float timeUntilFire;

    public AudioSource audioSource;
    public AudioClip fireSound;


    public float minPitch = 0.8f;
    public float maxPitch = 1.2f;

    private void Start()
    {
        apsBase = aps;
        targetingRangeBase = targetingRange;
        
        upgradeButton.onClick.AddListener(Upgrade);
    }
    
    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= 1f / aps)
        {
            Debug.Log("Slowed Enemies");
            FreezeEnemies();
            timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, Vector2.zero, 0f, enemyMask);

        if (hits.Length > 0)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            //make the audiosource play at half the volume
            audioSource.volume = 0.75f;
            //play the fire sound at the randomized pitch
            audioSource.PlayOneShot(fireSound);
            CreateDeathEffect();
            
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        
        em.ResetSpeed();
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

        aps = CalculateAPS();
        targetingRange = CalculateRange();
        
        CloseUpgradeUI();
        Debug.Log("New BPS: " + aps);
        Debug.Log("New targeting range: " + targetingRange);
        Debug.Log("New cost: " + CalculateCost());
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateAPS()
    {
        return apsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRange()
    {
        return targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    
    private void OnDrawGizmosSelected(){
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
