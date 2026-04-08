using UnityEngine;

public class Health : DeathEffectObject
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 25;
    
    [Header("Coin Drop")]
    [SerializeField, Range(0f, 1f)] private float coinDropChance = 0.25f;
    [SerializeField] private GameObject coinPrefab;
    
    public GameObject FloatingScorePrefab;
    
    private bool isDestroyed = false;

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0 && !isDestroyed)
        {
            isDestroyed = true;

            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);

            CreateDeathEffect();

            // floating score text
            FloatingText floatingText = FloatingScorePrefab.GetComponent<FloatingText>();
            floatingText.SetText(currencyWorth);
            Instantiate(FloatingScorePrefab, transform.position, Quaternion.identity);

            // coin
            TryDropCoin();

            Destroy(gameObject);
        }
    }
    
    private void TryDropCoin()
    {
        if (coinPrefab == null)
            return;

        if (Random.value <= coinDropChance)
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}
