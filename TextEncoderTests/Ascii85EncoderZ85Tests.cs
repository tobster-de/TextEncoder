using System;
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
            yield return new TestCaseData("\0\0\0\0", "00000");
            yield return new TestCaseData("A", "k(");
            yield return new TestCaseData("AB", "k%+");
            yield return new TestCaseData("ABC", "k%^{");
            yield return new TestCaseData("XYZ", "sxO/");
            yield return new TestCaseData("1234", "f!$Kw");
            yield return new TestCaseData("Test", "raQb)");
            yield return new TestCaseData("TestTest", "raQb)raQb)");
            yield return new TestCaseData("TestTestTest", "raQb)raQb)raQb)");
            yield return new TestCaseData("TestTestTestTest", "raQb)raQb)raQb)raQb)");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog.", "ra]?=ADL#9yAN8bz*c7ww]z]pyisxjB0byAwPw]nxK@r5vs0hwwn=9k");
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