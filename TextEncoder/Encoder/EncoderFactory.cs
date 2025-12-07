using System;

namespace TextEncoder.Encoder;

public static class EncoderFactory
{
    public static Base32Encoder GetEncoder(Base32Format format)
    {
        switch (format)
        {
            case Base32Format.RFC4648:
                return Base32Encoder.Rfc4648;
            case Base32Format.ZBase32:
                return Base32Encoder.ZBase32;
            case Base32Format.Crockford:
                return Base32Encoder.Crockford;
            case Base32Format.ExtendedHex:
                return Base32Encoder.ExtendedHex;
            default:
                throw new ArgumentOutOfRangeException(nameof(format), format, null);
        }
    }
    
    public static Base64Encoder GetEncoder(Base64Format format)
    {
        switch (format)
        {
            case Base64Format.Default:
                return Base64Encoder.Default;
            case Base64Format.WithoutPadding:
                return Base64Encoder.DefaultNoPadding;
            case Base64Format.UrlEncoding:
                return Base64Encoder.UrlEncoding;
            case Base64Format.XmlEncoding:
                return Base64Encoder.XmlEncoding;
            case Base64Format.RegExEncoding:
                return Base64Encoder.RegExEncoding;
            case Base64Format.FileEncoding:
                return Base64Encoder.FileEncoding;
            default:
                throw new ArgumentOutOfRangeException(nameof(format), format, null);
        }
    }
}