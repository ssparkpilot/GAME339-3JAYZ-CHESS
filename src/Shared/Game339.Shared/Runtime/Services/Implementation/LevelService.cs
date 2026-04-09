using System;

namespace Game339.Shared.Services.Implementation
{
    public class LevelService : ILevelService
    {
        public int IncreaseCurrency(int currency, int amount)
        {
            return currency + amount;
        }

        public bool CanSpendCurrency(int currency, int amount)
        {
            return currency >= amount;
        }

        public int SpendCurrency(int currency, int amount)
        {
            return currency - amount;
        }

        public int LoseHealth(int currentHealth, int amount)
        {
            return Math.Max(currentHealth - amount, 0);
        }

        public bool IsGameOver(int health)
        {
            return health <= 0;
        }
    }
}