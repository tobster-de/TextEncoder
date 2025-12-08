using System.Collections;
using NUnit.Framework;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class Base52EncoderTests : GenericTestBase<Base52Encoder>
{
    public Base52EncoderTests()
    {
        this.Subject = Base52Encoder.WithoutDigits;
    }

    public static IEnumerable TestData
    {
        get
        {
            yield return new TestCaseData("A", "BN");
            yield return new TestCaseData("AB", "GJO");
            yield return new TestCaseData("ABC", "eViL");
            yield return new TestCaseData("XYZ", "pJOy");
            yield return new TestCaseData("1234", "CIuBjI");
            yield return new TestCaseData("Test", "DliEcE");
            yield return new TestCaseData("TestTest", "qDgcHfnlwPo");
            yield return new TestCaseData("TestTestTest", "JHMYaYjxYEAoQltdA");
            yield return new TestCaseData("TestTestTestTest", "BzMbwxwSXRplexlbIOBIbQE");
            yield return new TestCaseData("The quick brown fox jumps over the lazy dog.", "GTaiuJJDdTNAaGyFOtjsiweptruLwssSkYSEhVIDszaFrTYsqBAKmDhJMSPevC");
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