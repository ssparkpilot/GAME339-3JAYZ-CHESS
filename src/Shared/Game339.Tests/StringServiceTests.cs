using Game339.Shared.Services.Implementation;
using NUnit.Framework;

namespace Game339.Tests;

public class StringServiceTests
{
    private StringService _svc;

    [SetUp]
    public void SetUp()
    {
        _svc = new StringService(EmptyGameLog.Instance);
    }

    [TestCase("hello", "olleh")]
    [TestCase("", "")]
    [TestCase("a", "a")]
    [TestCase("racecar", "racecar")]
    public void Reverse_ReturnsExpectedString(string input, string expected)
    {
        // Act
        var result = _svc.Reverse(input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Reverse_NullString_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<System.ArgumentNullException>(() => _svc.Reverse(null));
    }
    
    [TestCase("hello world", "world hello")]
    [TestCase("  big   large  space  ", "space large big")]
    [TestCase("racecar", "racecar")]
    [TestCase("", "")]
    public void ReverseWords_ReturnsExpectedString(string input, string expected)
    {
        // Act
        var result = _svc.ReverseWords(input);

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ReverseWords_NullString_ReturnsNull()
    {
        // Act
        var result = _svc.ReverseWords(null);

        // Assert
        Assert.That(result, Is.Null);
    }

}
