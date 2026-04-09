using Game339.Shared.Services;
using Game339.Shared.Services.Implementation;

namespace Game339.Tests;

public class HealthServiceTests
{
    private IHealthService _svc;

    [SetUp]
    public void Setup()
    {
        _svc = new HealthService();
    }

    [Test]
    public void ApplyDamage_Subtracts_Damage_From_Current_Health()
    {
        var result = _svc.ApplyDamage(currentHealth: 10, damage: 3);

        Assert.That(result, Is.EqualTo(7));
    }

    [Test]
    public void ApplyDamage_Allows_Health_To_Go_Negative()
    {
        var result = _svc.ApplyDamage(currentHealth: 5, damage: 10);

        Assert.That(result, Is.EqualTo(-5));
    }

    [Test]
    public void IsDead_Returns_True_When_Health_Is_Zero()
    {
        var result = _svc.IsDead(0);

        Assert.That(result, Is.True);
    }

    [Test]
    public void IsDead_Returns_False_When_Health_Is_Positive()
    {
        var result = _svc.IsDead(1);

        Assert.That(result, Is.False);
    }

    [Test]
    public void ShouldDropCoin_Returns_True_When_Random_Is_Less_Than_Chance()
    {
        var result = _svc.ShouldDropCoin(dropChance: 0.5f, randomValue: 0.25f);

        Assert.That(result, Is.True);
    }

    [Test]
    public void ShouldDropCoin_Returns_False_When_Random_Exceeds_Chance()
    {
        var result = _svc.ShouldDropCoin(dropChance: 0.25f, randomValue: 0.9f);

        Assert.That(result, Is.False);
    }
}