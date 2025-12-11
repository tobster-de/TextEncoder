using System;
using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base64EncoderDefaultTests : GenericTestBase<Base64Encoder>
{
    public Base64EncoderDefaultTests()
    {
        this.Subject = Base64Encoder.Default;
    }

    public static IEnumerable TestStrings
    {
        get
        {
            for (int i = 0; i < Count; i++) yield return new TestCaseData(Convert.ToBase64String(RandomData[i]));
        }
    }

    public static IEnumerable TestData
    {
        get
        {
            for (int i = 0; i < Count; i++) yield return new TestCaseData(RandomData[i]);
        }
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void Alternate_RoundTrip(byte[] d)
    {
        // Arrange

        // Act
        byte[] result = Base64Encoder.FromBase64String(Base64Encoder.ToBase64String(d));

        // Assert
        Assert.That(result, Is.EqualTo(d).AsCollection);
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void RoundTrip(byte[] d)
    {
        Assert.That(this.Subject!.FromBase(this.Subject!.ToBase(d)), Is.EqualTo(d).AsCollection);
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void ToBase64(byte[] d)
    {
        Assert.That(this.Subject!.ToBase(d), Is.EqualTo(Convert.ToBase64String(d)));
    }

    [Test, TestCaseSource(nameof(TestStrings))]
    public void FromBase64(string s)
    {
        Assert.That(this.Subject!.FromBase(s), Is.EqualTo(Convert.FromBase64String(s)).AsCollection);
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
        Assert.Throws<FormatException>(() => this.Subject!.Decode(encoded));
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

    [Test]
    public void ToBase_WithZeroes()
    {
        // Arrange
        byte[] data = [0, 0, 0, 0];

        // Act
        string result = this.Subject!.ToBase(data);

        // Assert
        Assert.That(result, Is.EqualTo("AAAAAA=="));
    }
}