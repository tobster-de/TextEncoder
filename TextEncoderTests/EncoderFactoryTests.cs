using System;
using System.Collections;
using NUnit.Framework;
using TextEncoder;
using TextEncoder.Encoder;

namespace TextEncoderTests;

[TestFixture]
public class EncoderFactoryTests
{

    public static IEnumerable FactoryBase32Cases
    {
        get
        {
            yield return new TestCaseData(Base32Format.RFC4648, Base32Encoder.Rfc4648);
            yield return new TestCaseData(Base32Format.Crockford, Base32Encoder.Crockford);
            yield return new TestCaseData(Base32Format.ExtendedHex, Base32Encoder.ExtendedHex);
            yield return new TestCaseData(Base32Format.ZBase32, Base32Encoder.ZBase32);
        }
    }

    [Test]
    [TestCaseSource(nameof(FactoryBase32Cases))]
    public void GetEncoder_WithBase32Format_ShouldReturnCorrectEncoder(Base32Format format, Base32Encoder expectedEncoder)
    {
        // Arrange

        // Act
        Base32Encoder encoder = EncoderFactory.GetEncoder(format);

        // Assert
        Assert.That(encoder, Is.EqualTo(expectedEncoder));
    }

    public static IEnumerable FactoryBase64Cases
    {
        get
        {
            yield return new TestCaseData(Base64Format.Default, Base64Encoder.Default);
            yield return new TestCaseData(Base64Format.WithoutPadding, Base64Encoder.DefaultNoPadding);
            yield return new TestCaseData(Base64Format.UrlEncoding, Base64Encoder.UrlEncoding);
            yield return new TestCaseData(Base64Format.XmlEncoding, Base64Encoder.XmlEncoding);
            yield return new TestCaseData(Base64Format.RegExEncoding, Base64Encoder.RegExEncoding);
            yield return new TestCaseData(Base64Format.FileEncoding, Base64Encoder.FileEncoding);
        }
    }

    [Test]
    [TestCaseSource(nameof(FactoryBase64Cases))]
    public void GetEncoder_WithBase64Format_ShouldReturnCorrectEncoder(Base64Format format, Base64Encoder expectedEncoder)
    {
        // Arrange

        // Act
        Base64Encoder encoder = EncoderFactory.GetEncoder(format);

        // Assert
        Assert.That(encoder, Is.EqualTo(expectedEncoder));
    }

    [Test]
    public void GetEncoder_WithInvalidBase32Format_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var invalidFormat = (Base32Format)999;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => EncoderFactory.GetEncoder(invalidFormat));
    }

    [Test]
    public void GetEncoder_WithInvalidBase64Format_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var invalidFormat = (Base64Format)999;

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => EncoderFactory.GetEncoder(invalidFormat));
    }
}
