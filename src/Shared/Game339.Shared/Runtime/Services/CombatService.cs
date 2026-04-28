using Game339.Shared.Models;

public class CombatService
{
    public void ResolveAttack(TowerUnit tower, EnemyUnit enemy)
    {
        enemy.ApplyDamage(tower.Damage);
    }
}