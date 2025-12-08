using System;
using System.Text;
using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

/// <summary>
/// Base32 utilizes 4 bit segments
/// </summary>
public class Base32Encoder : BaseEncoder
{
    private static Base32Encoder? _rfc4648;
    private static Base32Encoder? _zBase32;
    private static Base32Encoder? _extendedHex;
    private static Base32Encoder? _crockford;

    /// <summary>
    /// RFC4648 Base32 alphabet
    /// </summary>
    public static Base32Encoder Rfc4648 => _rfc4648 ??= new Base32Encoder(Base32Mapping.Rfc4648);
    /// <summary>
    /// Z-Base-32 alphabet
    /// </summary>
    public static Base32Encoder ZBase32 => _zBase32 ??= new Base32Encoder(Base32Mapping.ZBase32);
    /// <summary>
    /// Extended Hex alphabet
    /// </summary>
    public static Base32Encoder ExtendedHex => _extendedHex ??= new Base32Encoder(Base32Mapping.ExtendedHex);
    /// <summary>
    /// Crockford's Base32 alphabet
    /// </summary>
    public static Base32Encoder Crockford => _crockford ??= new Base32Encoder(Base32Mapping.Crockford);

    private readonly char[] _characterSet;
    private readonly byte[] _characterMap;
    private readonly char? _paddingChar;
    private readonly bool _usePadding;

    private Base32Encoder(ICharacterMapping characterMapping)
    {
        _characterSet = characterMapping.Characters;
        _characterMap = CreateCharacterMap(characterMapping.CharValues);

        _paddingChar = characterMapping.PaddingChar;
        _usePadding = characterMapping.PaddingChar.HasValue;
    }

    /// <inheritdoc />
    public override string ToBase(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return string.Empty;
        }

        StringBuilder result = new StringBuilder((data.Length * 8 + 4) / 5);
        int buffer = 0;
        int bitsLeft = 0;

        foreach (byte b in data)
        {
            buffer = (buffer << 8) | b;
            bitsLeft += 8;
            while (bitsLeft >= 5)
            {
                int index = (buffer >> (bitsLeft - 5)) & 0x1F;
                result.Append(_characterSet[index]);
                bitsLeft -= 5;
            }
        }

        if (bitsLeft > 0)
        {
            int index = (buffer << (5 - bitsLeft)) & 0x1F;
            result.Append(_characterSet[index]);
        }

        if (_usePadding)
        {
            while (result.Length % 8 != 0)
            {
                result.Append(_paddingChar!.Value);
            }
        }

        return result.ToString();
    }

    /// <inheritdoc />
    public override byte[] FromBase(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        int padding = 0;
        if (_usePadding)
        {
            for (padding = 0; padding < data.Length && data[data.Length - padding - 1] == _paddingChar; padding++) ;
        }

        // result size: 5 bits per char, 8 bits per byte
        byte[] result = new byte[(data.Length - padding) * 5 / 8];

        int p = 0;
        int buffer = 0;
        int bitsLeft = 0;

        foreach (char c in data)
        {
            // Handle padding: RFC4648 usually ends with padding. 
            // If we encounter padding, we assume the rest is padding and stop.
            if (_usePadding && c == _paddingChar)
            {
                break;
            }

            if (c >= _characterMap.Length)
            {
                throw new FormatException($"Invalid character '{c}' encountered.");
            }

            byte value = _characterMap[c];

            // invalid characters are initialized with 0xFF (255)
            if (value == 0xFF)
            {
                throw new FormatException($"Invalid character '{c}' encountered.");
            }

            buffer = (buffer << 5) | value;
            bitsLeft += 5;

            if (bitsLeft >= 8)
            {
                byte b = (byte)((buffer >> (bitsLeft - 8)) & 0xFF);
                result[p++] = b;
                bitsLeft -= 8;
            }
        }

        return result;
    }
}