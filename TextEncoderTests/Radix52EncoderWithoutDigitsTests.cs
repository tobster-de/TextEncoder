using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;
using TextEncoder.Radix;

namespace TextEncoderTests;

[TestFixture]
public class Radix52EncoderWithoutDigitsTests
{
    public RadixEncoder Subject { get; set; }

    public Radix52EncoderWithoutDigitsTests()
    {
        this.Subject = Radix52Encoder.WithoutDigits;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData(0, "A");
            yield return new TestCaseData(100, "Bw");
            yield return new TestCaseData(-100, "-Bw");
            yield return new TestCaseData(256, "Ew");
            yield return new TestCaseData(-256, "-Ew");
            yield return new TestCaseData(1024, "Tk");
            yield return new TestCaseData(-1024, "-Tk");
            yield return new TestCaseData(65536, "YMQ");
            yield return new TestCaseData(-65536, "-YMQ");
            yield return new TestCaseData(long.MaxValue, "BLptSBVAoOeH");
            yield return new TestCaseData(long.MinValue + 1, "-BLptSBVAoOeH");
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