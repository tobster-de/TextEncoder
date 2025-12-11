using System;
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
            yield return new TestCaseData("\0\0\0\0", "AAAA");
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
        Assert.That(result, Is.EqualTo(data).AsCollection, $"Expected {plain} but got {System.Text.Encoding.UTF8.GetString(result)}");
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