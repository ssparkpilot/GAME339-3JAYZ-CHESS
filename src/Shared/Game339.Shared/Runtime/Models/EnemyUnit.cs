using Game339.Shared.Models;
using Game339.Shared.Services;

public class EnemyUnit : Unit
{
    public int Health { get; private set; }
    public IChessMovementRule MovementRule { get; }

    public EnemyUnit(GridPosition pos, IChessMovementRule rule, int health)
        : base(pos, isEnemy: true)
    {
        MovementRule = rule;
        Health = health;
    }

    public void ApplyDamage(int amount)
    {
        Health -= amount;
    }

    public bool IsDead => Health <= 0;
}