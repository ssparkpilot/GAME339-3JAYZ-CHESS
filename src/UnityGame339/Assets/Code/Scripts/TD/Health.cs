using System;
using UnityEngine;
using Game339.Shared.Models;
using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;
using Random = UnityEngine.Random;

public class Health : DeathEffectObject
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 25;

    public int CurrentHP => hitPoints;

    public event Action<int> OnHealthChanged;
    public event Action OnDied;

    [Header("Coin Drop")]
    [SerializeField, Range(0f, 1f)] private float coinDropChance = 0.25f;
    [SerializeField] private GameObject coinPrefab;

    [Header("UI")]
    [SerializeField] private FloatingText floatingScorePrefab;

    private bool isDestroyed;
    private IHealthService healthService;

    // NEW: references for board cleanup
    private GridBoard board;
    private Unit unit;

    private void Awake()
    {
        healthService = new HealthService();
    }

    // Call this when the enemy is spawned / registered on the board
    public void InitializeGridReferences(GridBoard gridBoard, Unit owningUnit)
    {
        board = gridBoard;
        unit = owningUnit;
    }

    public void TakeDamage(int damage)
    {
        if (isDestroyed)
            return;

        hitPoints = healthService.ApplyDamage(hitPoints, damage);
        OnHealthChanged?.Invoke(hitPoints);

        if (healthService.IsDead(hitPoints))
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        // Clear board occupancy before destroy
        if (board != null && unit != null)
        {
            var tile = board.GetTile(unit.Position);
            if (tile != null && tile.Occupant == unit)
            {
                tile.Clear();
                Debug.Log($"Cleared tile at {unit.Position.X}, {unit.Position.Y}");
            }
        }

        OnDied?.Invoke();

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