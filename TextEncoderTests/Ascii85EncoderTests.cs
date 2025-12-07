using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Ascii85EncoderTests : GenericTestBase<Ascii85Encoder>
{
    public Ascii85EncoderTests()
    {
        this.Subject = Ascii85Encoder.Original;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("A", "5l");
            yield return new TestCaseData("AB", "5sb");
            yield return new TestCaseData("ABC", "5sdp");
            yield return new TestCaseData("XYZ", "=BSf");
            yield return new TestCaseData("1234", "0etOA");
            yield return new TestCaseData("Test", "<+U,m");
            yield return new TestCaseData("TestTest", "<+U,m<+U,m");
            yield return new TestCaseData("TestTestTest", "<+U,m<+U,m<+U,m");
            yield return new TestCaseData("TestTestTestTest", "<+U,m<+U,m<+U,m<+U,m");
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