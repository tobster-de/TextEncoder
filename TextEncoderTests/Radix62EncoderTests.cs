using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;
using TextEncoder.Radix;

namespace TextEncoderTests;

[TestFixture]
public class Radix62EncoderTests
{
    public RadixEncoder Subject { get; set; }

    public Radix62EncoderTests()
    {
        this.Subject = Radix62Encoder.Instance;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData(0, "A");
            yield return new TestCaseData(1, "B");
            yield return new TestCaseData(100, "Bm");
            yield return new TestCaseData(-100, "-Bm");
            yield return new TestCaseData(256, "EI");
            yield return new TestCaseData(-256, "-EI");
            yield return new TestCaseData(1024, "Qg");
            yield return new TestCaseData(-1024, "-Qg");
            yield return new TestCaseData(65536, "RDC");
            yield return new TestCaseData(-65536, "-RDC");
            yield return new TestCaseData(long.MaxValue, "K9VIxAiFIwH");
            yield return new TestCaseData(long.MinValue + 1, "-K9VIxAiFIwH");
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