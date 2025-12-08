using NUnit.Framework;
using System;
using TextEncoder.Radix;

namespace TextEncoderTests;

[TestFixture]
public class RadixEncoderTests
{
    // --- CONSTRUCTOR VALIDATION TESTS ---

    [Test]
    [TestCase(2)]
    [TestCase(10)]
    [TestCase(36)]
    public void Constructor_WithValidRadix_DoesNotThrow(int radix)
    {
        // Arrange: Define a valid radix.
        // Act: Attempt to create a new instance of the RadixEncoder.
        // Assert: Verify that no exception is thrown.
        Assert.DoesNotThrow(() => new RadixEncoder(radix));
    }

    [Test]
    [TestCase(1)] // Too small
    [TestCase(37)] // Too large (CharacterSet length is 36)
    public void Constructor_WithInvalidRadix_ThrowsArgumentOutOfRangeException(int radix)
    {
        // Arrange: Define an invalid radix.
        // Act / Assert: Verify that calling the constructor throws the expected exception.
        Assert.Throws<ArgumentOutOfRangeException>(() => new RadixEncoder(radix));
    }

    // --- ENCODE TESTS (long -> string) ---

    [Test]
    [TestCase(16, 255, "FF")]
    [TestCase(16, 10, "A")]
    [TestCase(2, 10, "1010")]
    [TestCase(8, 8, "10")]
    [TestCase(36, 35, "Z")]
    [TestCase(36, 36, "10")]
    public void Encode_PositiveValue_ReturnsCorrectString(int radix, long value, string expected)
    {
        // Arrange: Set up the encoder and expected output.
        var encoder = new RadixEncoder(radix);

        // Act: Perform the encoding.
        string result = encoder.Encode(value);

        // Assert: Check if the encoded string matches the expectation.
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Encode_ZeroValue_ReturnsZeroString()
    {
        // Arrange: Set up the encoder.
        var encoder = new RadixEncoder(10);
        const long value = 0;
        const string expected = "0";

        // Act: Perform the encoding.
        string result = encoder.Encode(value);

        // Assert: Check if the result is "0".
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Encode_NegativeValue_ReturnsStringWithSign()
    {
        // Arrange: Set up the encoder and input.
        var encoder = new RadixEncoder(16);
        const long value = -255;
        const string expected = "-FF";

        // Act: Perform the encoding.
        string result = encoder.Encode(value);

        // Assert: Check if the sign is present and the magnitude is correct.
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase(8, long.MaxValue, "777777777777777777777")]
    [TestCase(16, long.MaxValue, "7FFFFFFFFFFFFFFF")]
    [TestCase(32, long.MaxValue, "7VVVVVVVVVVVV")]
    [TestCase(36, long.MaxValue, "1Y2P0IJ32E8E7")]
    [TestCase(36, int.MaxValue, "ZIK0ZJ")]
    public void Encode_LargeNumber_Works(int radix, long input, string expected)
    {
        // Arrange: Set up the encoder and input.
        var encoder = new RadixEncoder(radix);

        // Act: Perform the encoding.
        string result = encoder.Encode(input);

        // Assert: Check if the sign is present and the magnitude is correct.
        Assert.AreEqual(expected, result);
    }

    // --- DECODE TESTS (string -> long) ---

    [Test]
    [TestCase(16, "FF", 255)]
    [TestCase(16, "ff", 255)] // Test Case-Insensitivity
    [TestCase(2, "1010", 10)]
    [TestCase(36, "Z", 35)]
    [TestCase(36, "z", 35)]
    [TestCase(36, "10", 36)]
    public void Decode_ValidString_ReturnsCorrectLong(int radix, string input, long expected)
    {
        // Arrange: Set up the encoder.
        var encoder = new RadixEncoder(radix);

        // Act: Perform the decoding.
        long result = encoder.Decode(input);

        // Assert: Check if the decoded long matches the expectation.
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Decode_NegativeString_ReturnsNegativeLong()
    {
        // Arrange: Set up the encoder and input.
        var encoder = new RadixEncoder(16);
        const string input = "-A";
        const long expected = -10;

        // Act: Perform the decoding.
        long result = encoder.Decode(input);

        // Assert: Check if the decoded value is negative and correct.
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Decode_NullOrEmptyInput_ThrowsArgumentException()
    {
        // Arrange: Set up the encoder.
        var encoder = new RadixEncoder(10);
        string nullValue = null!;

        // Act / Assert: Verify exceptions for null and empty/whitespace input.
        Assert.Throws<ArgumentException>(() => encoder.Decode(nullValue));
        Assert.Throws<ArgumentException>(() => encoder.Decode(""));
        Assert.Throws<ArgumentException>(() => encoder.Decode("   "));
    }

    [Test]
    public void Decode_InvalidCharacterForBase_ThrowsFormatException()
    {
        // Arrange: Encoder set for Binary (Base 2). Character '2' is invalid here.
        var encoder = new RadixEncoder(2);
        const string input = "10201";

        // Act / Assert: Expect a FormatException.
        Assert.Throws<FormatException>(() => encoder.Decode(input));
    }

    // --- ROUNDTRIP TEST ---

    [Test]
    [TestCase(10, 1234567)]
    [TestCase(16, 987654321)]
    [TestCase(2, 512)]
    [TestCase(36, -999999999)] // Check roundtrip with negatives
    public void Roundtrip_EncodeThenDecode_ReturnsOriginalValue(int radix, long originalValue)
    {
        // Arrange: Setup encoder and original value.
        var encoder = new RadixEncoder(radix);

        // Act: Encode, then Decode the result.
        string encoded = encoder.Encode(originalValue);
        long decoded = encoder.Decode(encoded);

        // Assert: The final decoded value must equal the original.
        Assert.That(decoded, Is.EqualTo(originalValue),
            $"Roundtrip failed for Radix {radix}. Encoded: {encoded}");
    }
}