using System;
using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

/// <summary>
/// Base64 algorithm uses 6 bit segments
/// </summary>
/// <remarks>
///		Based on http://www.csharp411.com/convert-binary-to-base64-string/
/// </remarks>
public class Base64Encoder : BaseEncoder
{
    private static Base64Encoder? _default;
    private static Base64Encoder? _defaultNoPadding;
    private static Base64Encoder? _urlEncoding;
    private static Base64Encoder? _xmlEncoding;
    private static Base64Encoder? _regExEncoding;
    private static Base64Encoder? _fileEncoding;

    /// <summary>
    /// Returns the default Base64 encoder.
    /// </summary>
    public static Base64Encoder Default => _default ??= new Base64Encoder(Base64Mapping.Default);
    /// <summary>
    /// Returns the default Base64 encoder without padding.
    /// </summary>
    public static Base64Encoder DefaultNoPadding => _defaultNoPadding ??= new Base64Encoder(Base64Mapping.DefaultNoPadding);
    /// <summary>
    /// Returns the Base64 encoder with URL safe characters.
    /// </summary>
    public static Base64Encoder UrlEncoding => _urlEncoding ??= new Base64Encoder(Base64Mapping.UrlEncoding);
    /// <summary>
    /// Returns the Base64 encoder with XML compatible characters.
    /// </summary>
    public static Base64Encoder XmlEncoding => _xmlEncoding ??= new Base64Encoder(Base64Mapping.XmlEncoding);
    /// <summary>
    /// Returns the Base64 encoder with RegEx compatible characters.
    /// </summary>
    public static Base64Encoder RegExEncoding => _regExEncoding ??= new Base64Encoder(Base64Mapping.RegExEncoding);
    /// <summary>
    /// Returns the Base64 encoder with file system safe characters.
    /// </summary>
    public static Base64Encoder FileEncoding => _fileEncoding ??= new Base64Encoder(Base64Mapping.FileEncoding);

    private readonly char[] _characterSet;
    private readonly byte[] _characterMap;
    private readonly bool _usePadding;
    private readonly char? _paddingChar;

    internal Base64Encoder(ICharacterMapping characterMapping)
    {
        _characterSet = characterMapping.Characters;
        _characterMap = CreateCharacterMap(characterMapping.CharValues);

        _paddingChar = characterMapping.PaddingChar;
        _usePadding = characterMapping.PaddingChar.HasValue;
    }

    /// <summary>
    /// Converts a Base64 string to a byte array. This method equals the <see cref="System.Convert.FromBase64String"/> method in naming.
    /// </summary>
    public static byte[] FromBase64String(string data)
    {
        return Default.FromBase(data);
    }

    /// <summary>
    /// Converts a byte array to a Base64 string. This method equals the <see cref="System.Convert.ToBase64String(byte[])"/> method in naming.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ToBase64String(byte[] data)
    {
        return Default.ToBase(data);
    }

    /// <inheritdoc/>
    public override string ToBase(byte[] data)
    {
        int length;
        if (0 == (length = data.Length))
        {
            return string.Empty;
        }

        unsafe
        {
            fixed (byte* d = data)
            fixed (char* cs = _characterSet)
            {
                byte* dp = d;

                int padding = length % 3;
                if (padding > 0)
                    padding = 3 - padding;
                int blocks = (length - 1) / 3 + 1;

                int l = blocks * 4;

                char[] result = new char[l];

                fixed (char* s = result)
                {
                    char* sp = s;
                    byte b1, b2, b3;

                    for (int i = 1; i < blocks; i++)
                    {
                        b1 = *dp++;
                        b2 = *dp++;
                        b3 = *dp++;

                        *sp++ = cs[(b1 & 0xFC) >> 2];
                        *sp++ = cs[(b2 & 0xF0) >> 4 | (b1 & 0x03) << 4];
                        *sp++ = cs[(b3 & 0xC0) >> 6 | (b2 & 0x0F) << 2];
                        *sp++ = cs[b3 & 0x3F];
                    }

                    bool pad2 = padding == 2;
                    bool pad1 = padding > 0;

                    b1 = *dp++;
                    b2 = pad2 ? (byte)0 : *dp++;
                    b3 = pad1 ? (byte)0 : *dp;

                    *sp++ = cs[(b1 & 0xFC) >> 2];
                    *sp++ = cs[(b2 & 0xF0) >> 4 | (b1 & 0x03) << 4];
                    *sp++ = pad2 ? '=' : cs[(b3 & 0xC0) >> 6 | (b2 & 0x0F) << 2];
                    *sp = pad1 ? '=' : cs[b3 & 0x3F];

                    if (!_usePadding)
                    {
                        if (pad2) l--;
                        if (pad1) l--;
                    }
                }

                return new string(result, 0, l);
            }
        }
    }

    /// <inheritdoc/>
    public override byte[] FromBase(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        int length = data.Length;
        for (int i = 0; i < length; i++)
        {
            if (data[i] == _paddingChar) continue;

            if (_characterMap[data[i]] == 0xFF)
            {
                throw new FormatException($"Invalid character '{data[i]}' encountered.");
            }
        }

        unsafe
        {
            fixed (char* p = data.ToCharArray())
            {
                char* p2 = p;

                int blocks = (length - 1) / 4 + 1;
                int bytes = blocks * 3;

                int padding = 0;

                if (_usePadding)
                {
                    if (length > 2 && p2[length - 2] == _paddingChar)
                    {
                        padding = 2;
                    }
                    else if (length > 1 && p2[length - 1] == _paddingChar)
                    {
                        padding = 1;
                    }
                }
                else
                {
                    padding = blocks * 4 - length;
                }

                byte[] result = new byte[bytes - padding];

                fixed (byte* d = result)
                {
                    byte temp1, temp2;
                    byte* dp = d;

                    for (int i = 1; i < blocks; i++)
                    {
                        temp1 = _characterMap[*p2++];
                        temp2 = _characterMap[*p2++];

                        *dp++ = (byte)((temp1 << 2) | ((temp2 & 0x30) >> 4));
                        temp1 = _characterMap[*p2++];
                        *dp++ = (byte)(((temp1 & 0x3C) >> 2) | ((temp2 & 0x0F) << 4));
                        temp2 = _characterMap[*p2++];
                        *dp++ = (byte)(((temp1 & 0x03) << 6) | temp2);
                    }

                    temp1 = _characterMap[*p2++];
                    temp2 = _characterMap[*p2++];

                    *dp++ = (byte)((temp1 << 2) | ((temp2 & 0x30) >> 4));

                    temp1 = _characterMap[*p2++];

                    if (padding != 2)
                    {
                        *dp++ = (byte)(((temp1 & 0x3C) >> 2) | ((temp2 & 0x0F) << 4));
                    }

                    temp2 = _characterMap[*p2];
                    if (padding == 0)
                    {
                        *dp = (byte)(((temp1 & 0x03) << 6) | temp2);
                    }
                }

                return result;
            }
        }
    }
}