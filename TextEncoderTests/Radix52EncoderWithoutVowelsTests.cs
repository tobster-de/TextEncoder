using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;
using TextEncoder.Radix;

namespace TextEncoderTests;

[TestFixture]
public class Radix52EncoderWithoutVowelsTests
{
    public RadixEncoder Subject { get; set; }

    public Radix52EncoderWithoutVowelsTests()
    {
        this.Subject = Radix52Encoder.WithoutVowels;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData(0, "0");
            yield return new TestCaseData(100, "1w");
            yield return new TestCaseData(-100, "-1w");
            yield return new TestCaseData(256, "4w");
            yield return new TestCaseData(-256, "-4w");
            yield return new TestCaseData(1024, "Mh");
            yield return new TestCaseData(-1024, "-Mh");
            yield return new TestCaseData(65536, "SDJ");
            yield return new TestCaseData(-65536, "-SDJ");
            yield return new TestCaseData(long.MaxValue, "1CnsL1P0mGZ7");
            yield return new TestCaseData(long.MinValue + 1, "-1CnsL1P0mGZ7");
        }
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void RoundTrip(long value, string encoded)
    {
        // Arrange

        // Act
        long result = this.Subject.Decode(this.Subject.Encode(value));

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void ToBase(long value, string encoded)
    {
        // Arrange

        // Act
        string result = this.Subject.Encode(value);

        // Assert
        Assert.That(result, Is.EqualTo(encoded));
    }

    [Test, TestCaseSource(nameof(TestData))]
    public void FromBase(long value, string encoded)
    {
        // Arrange

        // Act
        long result = this.Subject.Decode(encoded);

        // Assert
        Assert.That(result, Is.EqualTo(value));
    }
}