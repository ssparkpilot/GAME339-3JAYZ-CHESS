namespace Game339.Shared.Services
{
    public interface IHealthService
    {
        int ApplyDamage(int currentHealth, int damage);
        bool IsDead(int health);
        bool ShouldDropCoin(float dropChance, float randomValue);
    }
}