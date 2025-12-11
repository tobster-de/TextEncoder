using System;
using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base52EncoderWithoutVowelsTests : GenericTestBase<Base52Encoder>
{
    public Base52EncoderWithoutVowelsTests()
    {
        this.Subject = Base52Encoder.WithoutVowels;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("\0\0\0\0", "0000");
            yield return new TestCaseData("A", "1F");
            yield return new TestCaseData("AB", "69G");
            yield return new TestCaseData("ABC", "ZPfC");
            yield return new TestCaseData("XYZ", "n9Gy");
            yield return new TestCaseData("1234", "28t1g8");
            yield return new TestCaseData("Test", "3jf4X4");
            yield return new TestCaseData("TestTest", "p3cX7bljwHm");
            yield return new TestCaseData("TestTestTest", "97DSVSgxS40mJjsY0");
            yield return new TestCaseData("TestTestTestTest", "1zDWwxwLRKnjZxjW8G18WJ4");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog.", "6MVft993YMF0V6y5GsgrfwZnsqtCwrrLhSL4dP83rzV5qMSrp10Bk3d9DLHZv2");
        }
    }

    public static IEnumerable BinaryData
    {
        get
        {
            yield return new TestCaseData(new byte[] { 0, 0, 0, 0 }, "1111");
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