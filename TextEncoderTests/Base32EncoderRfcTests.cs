using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base32EncoderRfcTests : GenericTestBase<Base32Encoder>
{
    public Base32EncoderRfcTests()
    {
        this.Subject = Base32Encoder.Rfc4648;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("A", "IE======");
            yield return new TestCaseData("AB", "IFBA====");
            yield return new TestCaseData("ABC", "IFBEG===");
            yield return new TestCaseData("XYZ", "LBMVU===");
            yield return new TestCaseData("1234", "GEZDGNA=");
            yield return new TestCaseData("Test", "KRSXG5A=");
            yield return new TestCaseData("TestTest", "KRSXG5CUMVZXI===");
            yield return new TestCaseData("TestTestTest", "KRSXG5CUMVZXIVDFON2A====");
            yield return new TestCaseData("TestTestTestTest", "KRSXG5CUMVZXIVDFON2FIZLTOQ======");
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
}