using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base32EncoderHexTests : GenericTestBase<Base32Encoder>
{
    public Base32EncoderHexTests()
    {
        this.Subject = Base32Encoder.ExtendedHex;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("A", "84======");
            yield return new TestCaseData("AB", "8510====");
            yield return new TestCaseData("ABC", "85146===");
            yield return new TestCaseData("XYZ", "B1CLK===");
            yield return new TestCaseData("1234", "64P36D0=");
            yield return new TestCaseData("Test", "AHIN6T0=");
            yield return new TestCaseData("TestTest", "AHIN6T2KCLPN8===");
            yield return new TestCaseData("TestTestTest", "AHIN6T2KCLPN8L35EDQ0====");
            yield return new TestCaseData("TestTestTestTest", "AHIN6T2KCLPN8L35EDQ58PBJEG======");
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
        CollectionAssert.AreEqual(data, result);
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void ToBase(string plain, string encoded)
    {
        // Arrange
        byte[] data = System.Text.Encoding.UTF8.GetBytes(plain);

        // Act
        string result = this.Subject!.ToBase(data);

        // Assert
        Assert.AreEqual(encoded, result);
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void FromBase(string plain, string encoded)
    {
        // Arrange
        byte[] data = System.Text.Encoding.UTF8.GetBytes(plain);

        // Act
        byte[] result = this.Subject!.FromBase(encoded);

        // Assert
        CollectionAssert.AreEqual(data, result);
    }
}