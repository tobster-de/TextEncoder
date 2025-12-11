using System;
using System.Text;
using NUnit.Framework;
using TextEncoder;

namespace TextEncoderTests;

[TestFixture]
public class Base64Tests
{
    [Test]
    public void Constructor_WithByteData_ShouldCreateCorrectInstance()
    {
        // Arrange
        byte[] data = Encoding.ASCII.GetBytes("Hello");
        string expectedValue = "SGVsbG8="; // Base64 representation of "Hello"

        // Act
        var base64 = new Base64(data);

        // Assert
        Assert.That(base64.Raw, Is.EqualTo(data));
        Assert.That(base64.Value, Is.EqualTo(expectedValue));
        Assert.That(base64.Format, Is.EqualTo(Base64Format.Default));
    }

    [Test]
    public void Parse_WithValidString_ShouldReturnBase64Object()
    {
        // Arrange
        string input = "SGVsbG8=";
        byte[] expectedBytes = Encoding.ASCII.GetBytes("Hello");

        // Act
        var result = Base64.Parse(input);

        // Assert
        Assert.That(result.Value, Is.EqualTo(input));
        Assert.That(result.Raw, Is.EqualTo(expectedBytes));
        Assert.That(result.Format, Is.EqualTo(Base64Format.Default));
    }

    [Test]
    public void Parse_WithNullString_ShouldThrowArgumentNullException()
    {
        // Arrange
        string? input = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => Base64.Parse(input!));
    }

    [Test]
    public void TryParse_WithValidString_ShouldReturnTrueAndInstance()
    {
        // Arrange
        string input = "SGVsbG8=";

        // Act
        bool success = Base64.TryParse(input, out Base64? result);

        // Assert
        Assert.That(success, Is.True);
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Value, Is.EqualTo(input));
    }

    [Test]
    public void TryParse_WithInvalidString_ShouldReturnFalseAndNull()
    {
        // Arrange
        string input = "Invalid Base64 String!";

        // Act
        bool success = Base64.TryParse(input, out Base64? result);

        // Assert
        Assert.That(success, Is.False);
        Assert.That(result, Is.Null);
    }

    [Test]
    public void Equals_WithOtherType_ShouldReturnFalse()
    {
        // Arrange
        var base64Instance = new Base64(Encoding.ASCII.GetBytes("Test1"));
        object obj = new object();

        // Act
        bool areEqual = base64Instance.Equals(obj);

        // Assert
        Assert.That(areEqual, Is.False);
    }

    [Test]
    public void Equals_WithSameValueAndFormat_ShouldReturnTrue()
    {
        // Arrange
        byte[] data = Encoding.ASCII.GetBytes("Test");
        var base64Instance1 = new Base64(data);
        var base64Instance2 = new Base64(data);

        // Act
        bool areEqual = base64Instance1.Equals(base64Instance2);

        // Assert
        Assert.That(areEqual, Is.True);
    }

    [Test]
    public void Equals_WithDifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var base64Instance1 = new Base64(Encoding.ASCII.GetBytes("Test1"));
        var base64Instance2 = new Base64(Encoding.ASCII.GetBytes("Test2"));

        // Act
        bool areEqual = base64Instance1.Equals(base64Instance2);

        // Assert
        Assert.That(areEqual, Is.False);
    }

    [Test]
    public void GetHashCode_WithSameValue_ShouldReturnSameHashCode()
    {
        // Arrange
        byte[] data = Encoding.ASCII.GetBytes("Test");
        var base64Instance1 = new Base64(data);
        var base64Instance2 = new Base64(data);

        // Act
        int hash1 = base64Instance1.GetHashCode();
        int hash2 = base64Instance2.GetHashCode();

        // Assert
        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void ToString_ShouldReturnEncodedString()
    {
        // Arrange
        string expected = "SGVsbG8=";
        byte[] data = Encoding.ASCII.GetBytes("Hello");
        var base64 = new Base64(data);

        // Act
        string result = base64.ToString();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }
}
