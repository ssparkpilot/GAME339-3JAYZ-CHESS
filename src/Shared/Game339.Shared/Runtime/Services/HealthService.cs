namespace Game339.Shared.Services.Implementation
{
    public class HealthService : IHealthService
    {
        public int ApplyDamage(int currentHealth, int damage)
        {
            return currentHealth - damage;
        }

        public bool IsDead(int health)
        {
            return health <= 0;
        }

        public bool ShouldDropCoin(float dropChance, float randomValue)
        {
            return randomValue <= dropChance;
        }
    }
}