using System;
using System.Text;
using NUnit.Framework;
using TextEncoder;

namespace TextEncoderTests;

[TestFixture]
public class Base32Tests
{
    [Test]
    public void Constructor_WithByteData_ShouldCreateCorrectInstance()
    {
        // Arrange
        byte[] data = Encoding.ASCII.GetBytes("Hello");
        string expectedValue = "JBSWY3DP"; // RFC4648 representation of "Hello"

        // Act
        var base32 = new Base32(data);

        // Assert
        Assert.That(base32.Raw, Is.EqualTo(data));
        Assert.That(base32.Value, Is.EqualTo(expectedValue));
        Assert.That(base32.Format, Is.EqualTo(Base32Format.RFC4648));
    }

    [Test]
    public void Parse_WithValidString_ShouldReturnBase32Object()
    {
        // Arrange
        string input = "JBSWY3DP";
        byte[] expectedBytes = Encoding.ASCII.GetBytes("Hello");

        // Act
        var result = Base32.Parse(input);

        // Assert
        Assert.That(result.Value, Is.EqualTo(input));
        Assert.That(result.Raw, Is.EqualTo(expectedBytes));
        Assert.That(result.Format, Is.EqualTo(Base32Format.RFC4648));
    }

    [Test]
    public void Parse_WithNullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string? input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Base32.Parse(input!));
    }

    [Test]
    public void TryParse_WithValidString_ShouldReturnTrueAndInstance()
    {
        // Arrange
        string input = "JBSWY3DP";

        // Act
        bool success = Base32.TryParse(input, out Base32? result);

        // Assert
        Assert.That(success, Is.True);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(input));
    }

    [Test]
    public void TryParse_WithInvalidString_ShouldReturnFalseAndNull()
    {
        // Arrange
        string input = "InvalidBase32String!"; // Contains invalid characters for default RFC4648

        // Act
        bool success = Base32.TryParse(input, out Base32? result);

        // Assert
        Assert.That(success, Is.False);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Equals_WithOtherType_ShouldReturnFalse()
    {
        // Arrange
        var base32Instance = new Base32(Encoding.ASCII.GetBytes("Test1"));
        object base32Instance2 = new object();

        // Act
        bool areEqual = base32Instance.Equals(base32Instance2);

        // Assert
        Assert.That(areEqual, Is.False);
    }

    [Test]
    public void Equals_WithSameValueAndFormat_ShouldReturnTrue()
    {
        // Arrange
        byte[] data = Encoding.ASCII.GetBytes("Test");
        var base32Instance1 = new Base32(data);
        var base32Instance2 = new Base32(data);

        // Act
        bool areEqual = base32Instance1.Equals(base32Instance2);

        // Assert
        Assert.That(areEqual, Is.True);
    }

    [Test]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var base32Instance1 = new Base32(Encoding.ASCII.GetBytes("Test1"));
        var base32Instance2 = new Base32(Encoding.ASCII.GetBytes("Test2"));

        // Act
        bool areEqual = base32Instance1.Equals(base32Instance2);

        // Assert
        Assert.That(areEqual, Is.False);
    }

    [Test]
    public void GetHashCode_WithSameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        byte[] data = Encoding.ASCII.GetBytes("Test");
        var base32Instance1 = new Base32(data);
        var base32Instance2 = new Base32(data);

        // Act
        int hash1 = base32Instance1.GetHashCode();
        int hash2 = base32Instance2.GetHashCode();

        // Assert
        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void ToString_ShouldReturnEncodedString()
    {
        // Arrange
        string expected = "JBSWY3DP";
        byte[] data = Encoding.ASCII.GetBytes("Hello");
        var base32 = new Base32(data);

        // Act
        string result = base32.ToString();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
