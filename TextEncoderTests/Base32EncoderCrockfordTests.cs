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
            yield return new TestCaseData("A", "84");
            yield return new TestCaseData("AB", "8510");
            yield return new TestCaseData("ABC", "85146");
            yield return new TestCaseData("XYZ", "B1CNM");
            yield return new TestCaseData("1234", "64S36D0");
            yield return new TestCaseData("Test", "AHJQ6X0");
            yield return new TestCaseData("TestTest", "AHJQ6X2MCNSQ8");
            yield return new TestCaseData("TestTestTest", "AHJQ6X2MCNSQ8N35EDT0");
            yield return new TestCaseData("TestTestTestTest", "AHJQ6X2MCNSQ8N35EDT58SBKEG");
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

        // Assert
        Assert.That(result, Is.EqualTo(data).AsCollection);
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
}