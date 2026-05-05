using System;
using UnityEngine;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using Random = UnityEngine.Random;

public class Health : DeathEffectObject
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 25;
    
    public int CurrentHP => hitPoints;
  
    public event Action<int> OnHealthChanged; // Event

    [Header("Coin Drop")]
    [SerializeField, Range(0f, 1f)] private float coinDropChance = 0.25f;
    [SerializeField] private GameObject coinPrefab;

    [Header("UI")]
    [SerializeField] private FloatingText floatingScorePrefab;

    private bool isDestroyed;
    private IHealthService healthService;

    private void Awake()
    {
        healthService = new HealthService();
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed)
            return;
        
        hitPoints = healthService.ApplyDamage(hitPoints, damage);
      
        OnHealthChanged?.Invoke(hitPoints); // Notify listeners
        
        if (healthService.IsDead(hitPoints))
        {
            Die();
        }
    }

    private void Die()
    {
        isDestroyed = true;

        EnemySpawner.onEnemyDestroy.Invoke();
        LevelManager.main.IncreaseCurrency(currencyWorth);

        CreateDeathEffect();
        SpawnFloatingScore();

        TryDropCoin();

        Destroy(gameObject);
    }

    private void SpawnFloatingScore()
    {
        var ft = Instantiate(
            floatingScorePrefab,
            transform.position,
            Quaternion.identity
        );

        ft.SetText(currencyWorth);
    }

    private void TryDropCoin()
    {
        if (coinPrefab == null)
            return;

        if (healthService.ShouldDropCoin(coinDropChance, Random.value))
        {
            Instantiate(coinPrefab, transform.position, Quaternion.identity);
        }
    }
}