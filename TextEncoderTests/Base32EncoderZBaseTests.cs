using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base32EncoderZBaseTests : GenericTestBase<Base32Encoder>
{
    public Base32EncoderZBaseTests()
    {
        this.Subject = Base32Encoder.ZBase32;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("A", "er");
            yield return new TestCaseData("AB", "efby");
            yield return new TestCaseData("ABC", "efbrg");
            yield return new TestCaseData("XYZ", "mbciw");
            yield return new TestCaseData("1234", "gr3dgpy");
            yield return new TestCaseData("Test", "kt1zg7y");
            yield return new TestCaseData("TestTest", "kt1zg7nwci3ze");
            yield return new TestCaseData("TestTestTest", "kt1zg7nwci3zeidfqp4y");
            yield return new TestCaseData("TestTestTestTest", "kt1zg7nwci3zeidfqp4fe3muqo");
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