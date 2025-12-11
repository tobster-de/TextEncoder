using System;
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
            yield return new TestCaseData("\0\0\0\0", "z");
            yield return new TestCaseData("A", "5l");
            yield return new TestCaseData("AB", "5sb");
            yield return new TestCaseData("ABC", "5sdp");
            yield return new TestCaseData("XYZ", "=BSf");
            yield return new TestCaseData("1234", "0etOA");
            yield return new TestCaseData("Test", "<+U,m");
            yield return new TestCaseData("TestTest", "<+U,m<+U,m");
            yield return new TestCaseData("TestTestTest", "<+U,m<+U,m<+U,m");
            yield return new TestCaseData("TestTestTestTest", "<+U,m<+U,m<+U,m<+U,m");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog.", "<+ohcEHPu*CER),Dg-(AAoDo:C3=B4F!,CEATAo8BOr<&@=!2AA8c*5");
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
    public void FromBase_WithWhitespace()
    {
        // Arrange
        string encoded = "<+U,m    <+U,m";

        // Act
        byte[] result = this.Subject!.FromBase(encoded);

        // Assert
        Assert.That(result, Is.EqualTo(System.Text.Encoding.UTF8.GetBytes("TestTest")));
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
    public void FromBase_WithShortInput()
    {
        // Arrange
        string encoded = "a";

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