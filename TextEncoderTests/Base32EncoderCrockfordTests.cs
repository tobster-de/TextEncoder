using System;
using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base32EncoderCrockfordTests : GenericTestBase<Base32Encoder>
{
    public Base32EncoderCrockfordTests()
    {
        this.Subject = Base32Encoder.Crockford;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("\0\0\0\0", "0000000");
            yield return new TestCaseData("A", "84");
            yield return new TestCaseData("AB", "8510");
            yield return new TestCaseData("ABC", "85146");
            yield return new TestCaseData("XYZ", "B1CNM");
            yield return new TestCaseData("1234", "64S36D0");
            yield return new TestCaseData("Test", "AHJQ6X0");
            yield return new TestCaseData("TestTest", "AHJQ6X2MCNSQ8");
            yield return new TestCaseData("TestTestTest", "AHJQ6X2MCNSQ8N35EDT0");
            yield return new TestCaseData("TestTestTestTest", "AHJQ6X2MCNSQ8N35EDT58SBKEG");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog.", "AHM6A83HENMP6TS0C9S6YXVE41K6YY10D9TPTW3K41QQCSBJ41T6GS90DHGQMY90CHQPEBG");
        }
    }
    
    public static IEnumerable MistakenCharactersTestData
    {
        get
        {
            yield return new TestCaseData("AB", "851O");
            yield return new TestCaseData("AB", "851o");
            yield return new TestCaseData("ABC", "85l46");
            yield return new TestCaseData("ABC", "85L46");
            yield return new TestCaseData("ABC", "85i46");
            yield return new TestCaseData("ABC", "85I46");
        }
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void RoundTrip(string plain, string encoded)
    {
        // Arrange
        byte[] data = System.Text.Encoding.UTF8.GetBytes(plain);

        // Act
        byte[] result = this.Subject!.FromBase(this.Subject!.ToBase(data));
        string asString = this.Subject!.Decode(this.Subject!.Encode(plain));

        // Assert
        Assert.That(result, Is.EqualTo(data).AsCollection);
        Assert.That(asString, Is.EqualTo(plain));
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void ToBase(string plain, string encoded)
    {
        // Arrange
        byte[] data = System.Text.Encoding.UTF8.GetBytes(plain);

        // Act
        string result = this.Subject!.ToBase(data);

        // Assert
        Assert.That(result, Is.EqualTo(encoded));
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void FromBase(string plain, string encoded)
    {
        // Arrange
        byte[] data = System.Text.Encoding.UTF8.GetBytes(plain);

        // Act
        byte[] result = this.Subject!.FromBase(encoded);

        // Assert
        Assert.That(result, Is.EqualTo(data).AsCollection);
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void FromBase_InLowerCase(string plain, string encoded)
    {
        // Arrange
        byte[] data = System.Text.Encoding.UTF8.GetBytes(plain);

        // Act
        byte[] result = this.Subject!.FromBase(encoded.ToLower());

        // Assert
        Assert.That(result, Is.EqualTo(data).AsCollection, $"Result: {Subject.Decode(encoded.ToLower())}");
    }

    [Test, TestCaseSource(nameof(MistakenCharactersTestData))]
    public void FromBase_WithMistakerCharacters(string plain, string encoded)
    {
        // Arrange
        byte[] data = System.Text.Encoding.UTF8.GetBytes(plain);

        // Act
        byte[] result = this.Subject!.FromBase(encoded);

        // Assert
        Assert.That(result, Is.EqualTo(data).AsCollection);
    }

    [Test]
    public void FromBase_WithEmptyInput()
    {
        // Arrange
        string encoded = string.Empty;

        // Act
        byte[] result = this.Subject!.FromBase(encoded);

        // Assert
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void FromBase_WithInvalidInput()
    {
        // Arrange
        string encoded = "abcdeöfghij";

        // Act / Assert
        Assert.Throws<FormatException>(() => this.Subject!.FromBase(encoded));
    }

    [Test]
    public void ToBase_WithEmptyInput()
    {
        // Arrange
        byte[] data = [];

        // Act
        string result = this.Subject!.ToBase(data);

        // Assert
        Assert.That(result, Is.Empty);
    }
}