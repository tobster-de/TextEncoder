using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base62EncoderTests : GenericTestBase<Base62Encoder>
{
    public Base62EncoderTests()
    {
        this.Subject = Base62Encoder.Instance;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("A", "BD");
            yield return new TestCaseData("AB", "EVc");
            yield return new TestCaseData("ABC", "R6kr");
            yield return new TestCaseData("XYZ", "YSPw");
            yield return new TestCaseData("1234", "31LVq");
            yield return new TestCaseData("Test", "BhzHUo");
            yield return new TestCaseData("TestTest", "HPO0kcI9QZO");
            yield return new TestCaseData("TestTestTest", "h8GS3SUlvOPHTbpi");
            yield return new TestCaseData("TestTestTestTest", "CjPsiqd87l9KMOP3u88Kk0");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog.", "hQZVZoqGqAfM4jMcpqIVUOjYLfPaHJf59bIpCYvNsMqFc6LTNwo0Gx82U3c");
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
        Assert.That(result, Is.EqualTo(data).AsCollection, $"Expected {plain} but got {System.Text.Encoding.UTF8.GetString(result)}");
    }
}