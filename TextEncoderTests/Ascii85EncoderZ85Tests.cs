using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Ascii85EncoderZ85Tests : GenericTestBase<Ascii85Encoder>
{
    public Ascii85EncoderZ85Tests()
    {
        this.Subject = Ascii85Encoder.ZeroMq;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("A", "k(");
            yield return new TestCaseData("AB", "k%+");
            yield return new TestCaseData("ABC", "k%^{");
            yield return new TestCaseData("XYZ", "sxO/");
            yield return new TestCaseData("1234", "f!$Kw");
            yield return new TestCaseData("Test", "raQb)");
            yield return new TestCaseData("TestTest", "raQb)raQb)");
            yield return new TestCaseData("TestTestTest", "raQb)raQb)raQb)");
            yield return new TestCaseData("TestTestTestTest", "raQb)raQb)raQb)raQb)");
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