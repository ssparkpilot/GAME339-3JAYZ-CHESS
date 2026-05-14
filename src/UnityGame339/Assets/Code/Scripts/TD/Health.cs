using System;
using UnityEngine;

public class Health : DeathEffectObject
{
    [Header("Health")]
    [SerializeField] private int maxHP = 20;
    [SerializeField] private int currentHP;

    [Header("Rewards")]
    [SerializeField] private int currencyWorth = 0;

    private bool isDestroyed;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    public event Action<int> OnHealthChanged;
    public event Action OnDied;

    private void Awake()
    {
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP);
    }

    private void Start()
    {
        OnHealthChanged?.Invoke(currentHP);
    }

    public void TakeDamage(int amount)
    {
        if (isDestroyed)
            return;

        currentHP -= amount;
        currentHP = Mathf.Max(currentHP, 0);

        Debug.Log(gameObject.name + " HP: " + currentHP);

        OnHealthChanged?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDestroyed)
            return;

        currentHP += amount;
        currentHP = Mathf.Min(currentHP, maxHP);

        OnHealthChanged?.Invoke(currentHP);
    }

    private void Die()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;

        if (EnemyWaveSpawner.main != null)
        {
            EnemyWaveSpawner.main.RegisterEnemyDefeated(gameObject);
        }

        if (LevelManager.main != null && currencyWorth > 0)
        {
            LevelManager.main.IncreaseCurrency(currencyWorth);
        }

        OnDied?.Invoke();

        if (EffectPrefab != null)
        {
            CreateDeathEffect();
        }

        Destroy(gameObject);
    }
}