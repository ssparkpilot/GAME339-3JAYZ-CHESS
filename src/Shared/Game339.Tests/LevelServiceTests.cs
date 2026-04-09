using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;

namespace Game339.Tests;

public class LevelServiceTests
{
    private ILevelService _svc;

    [SetUp]
    public void Setup()
    {
        _svc = new LevelService();
    }

    [Test]
    public void IncreaseCurrency_Adds_Amount()
    {
        var result = _svc.IncreaseCurrency(currency: 100, amount: 25);
        Assert.That(result, Is.EqualTo(125));
    }

    [Test]
    public void CanSpendCurrency_Returns_True_When_Enough()
    {
        var result = _svc.CanSpendCurrency(currency: 100, amount: 50);
        Assert.That(result, Is.True);
    }

    [Test]
    public void CanSpendCurrency_Returns_False_When_Not_Enough()
    {
        var result = _svc.CanSpendCurrency(currency: 25, amount: 50);
        Assert.That(result, Is.False);
    }

    [Test]
    public void SpendCurrency_Subtracts_Amount()
    {
        var result = _svc.SpendCurrency(currency: 100, amount: 40);
        Assert.That(result, Is.EqualTo(60));
    }

    [Test]
    public void LoseHealth_Reduces_Health()
    {
        var result = _svc.LoseHealth(currentHealth: 100, amount: 30);
        Assert.That(result, Is.EqualTo(70));
    }

    [Test]
    public void LoseHealth_Does_Not_Go_Below_Zero()
    {
        var result = _svc.LoseHealth(currentHealth: 10, amount: 50);
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void IsGameOver_Returns_True_When_Health_Zero()
    {
        var result = _svc.IsGameOver(0);
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsGameOver_Returns_False_When_Health_Positive()
    {
        var result = _svc.IsGameOver(1);
        Assert.That(result, Is.False);
    }
}