using System;
using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

public abstract class BaseEncoderWithCustomCharset : BaseEncoder
{
    private readonly char[] _characterSet;
    private readonly byte[] _characterMap;

    private readonly double _factorEncode;
    private readonly double _factorDecode;
    private readonly int _radix;

    internal BaseEncoderWithCustomCharset(ICharacterMapping characterMapping)
    {
        _characterSet = characterMapping.Characters;
        _characterMap = CreateCharacterMap(characterMapping.CharValues);
        _radix = characterMapping.Characters.Length;

        double l256 = Math.Log(256);
        double lradix = Math.Log(_radix);

        // log(256) / log(radix) is factor for approximated encoding length
        _factorEncode = l256 / lradix;

        // log(radix) / log(256) is factor for approximated encoding length
        _factorDecode = lradix / l256;
    }

    /// <inheritdoc />
    public override string ToBase(byte[] data)
    {
        if (data.Length == 0)
        {
            return string.Empty;
        }

        // Convert byte array to a large integer (BigInteger requires little-endian, unsigned)
        // We reverse the part after leading zeros because BigInteger expects little-endian
        // and usually byte arrays for encoding are treated as big-endian numbers.
        // However, a simpler approach for Base58 is treating the whole array as a Big Endian number.
        // Let's do the manual division method which is standard for Base58 implementations
        // to ensure correct handling of leading zeros without BigInteger complexity regarding sign bit.

        // Allocate enough space for the result
        int size = (int)(data.Length * _factorEncode) + 1;
        byte[] buffer = new byte[size];
        int length = 0;

        for (int i = 0; i < data.Length; i++)
        {
            int carry = data[i];
            for (int j = 0; j < length; j++)
            {
                carry += buffer[j] << 8;
                buffer[j] = (byte)(carry % _radix);
                carry /= _radix;
            }

            while (carry > 0)
            {
                buffer[length++] = (byte)(carry % _radix);
                carry /= _radix;
            }
        }

        // Construct the string
        char[] result = new char[length];

        // Add the encoded characters (reverse the buffer)
        for (int i = 0; i < length; i++)
        {
            result[length - 1 - i] = _characterSet[buffer[i]];
        }

        return new string(result);
    }

    /// <inheritdoc />
    public override byte[] FromBase(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return [];
        }

        // Allocate enough space for the result
        int size = (int)(data.Length * _factorDecode) + 1;
        byte[] buffer = new byte[size];
        int length = 0;

        for (int i = 0; i < data.Length; i++)
        {
            char c = data[i];
            int digit = _characterMap[c];
            if (digit == 0xFF)
            {
                throw new FormatException($"Invalid character '{c}' at position {i}");
            }

            int carry = digit;
            for (int j = 0; j < length; j++)
            {
                carry += buffer[j] * _radix;
                buffer[j] = (byte)(carry & 0xFF);
                carry >>= 8;
            }

            while (carry > 0)
            {
                buffer[length++] = (byte)(carry & 0xFF);
                carry >>= 8;
            }
        }

        // Remove trailing zeros from buffer (which are leading zeros in the big-endian result)
        // The buffer is currently little-endian
        byte[] result = new byte[length];

        // 1. Leading zeros are already 0 in the new array

        // 2. Copy the decoded bytes in reverse order
        for (int i = 0; i < length; i++)
        {
            result[length - 1 - i] = buffer[i];
        }

        return result;
    }
}