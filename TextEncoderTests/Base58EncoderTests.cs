using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base58EncoderTests : GenericTestBase<Base58Encoder>
{
    public Base58EncoderTests()
    {
        this.Subject = Base58Encoder.Instance;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("A", "28");
            yield return new TestCaseData("AB", "5y3");
            yield return new TestCaseData("ABC", "NvLz");
            yield return new TestCaseData("XYZ", "WgBK");
            yield return new TestCaseData("1234", "2FwFnT");
            yield return new TestCaseData("Test", "3A836b");
            yield return new TestCaseData("TestTest", "F7kVCJSZXKy");
            yield return new TestCaseData("TestTestTest", "2bNcNLF1HWfuXwN43");
            yield return new TestCaseData("TestTestTestTest", "BRTJxkVxjAu9KBGGWFZvD9");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog.", "USm3fpXnKG5EUBx2ndxBDMPVciP5hGey2Jh4NDv6gmeo1LkMeiKrLJUUBk6Z");
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