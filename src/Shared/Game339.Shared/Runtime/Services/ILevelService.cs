namespace Game339.Shared.Services
{
    public interface ILevelService
    {
        int IncreaseCurrency(int currency, int amount);
        bool CanSpendCurrency(int currency, int amount);
        int SpendCurrency(int currency, int amount);

        int LoseHealth(int currentHealth, int amount);
        bool IsGameOver(int health);
    }
}