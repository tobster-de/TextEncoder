using System;

namespace TextEncoder.Encoder;

/// <summary>
/// Like Base64 but excludes non-alphanumeric characters (+ and /) and pairs of characters that often
/// look ambiguous when rendered: zero (0) and capital-O (O), and capital-I (I) and lowercase-L (l).
/// </summary>
public class Base58Encoder : BaseEncoder
{
    public static readonly Base58Encoder Instance = new();
    
    private const string CharacterSet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

    private Base58Encoder()
    {
    }
    
    /// <inheritdoc />
    public override string ToBase(byte[] data)
    {
        if (data.Length == 0)
        {
            return string.Empty;
        }

        // Count leading zeros
        int zeros = 0;
        while (zeros < data.Length && data[zeros] == 0)
        {
            zeros++;
        }

        // Convert byte array to a large integer (BigInteger requires little-endian, unsigned)
        // We reverse the part after leading zeros because BigInteger expects little-endian
        // and usually byte arrays for encoding are treated as big-endian numbers.
        // However, a simpler approach for Base58 is treating the whole array as a Big Endian number.
        // Let's do the manual division method which is standard for Base58 implementations
        // to ensure correct handling of leading zeros without BigInteger complexity regarding sign bit.

        // Allocate enough space for the result
        int size = (data.Length - zeros) * 138 / 100 + 1; // log(256) / log(58) is approx 1.37
        byte[] buffer = new byte[size];
        int length = 0;

        for (int i = zeros; i < data.Length; i++)
        {
            int carry = data[i];
            for (int j = 0; j < length; j++)
            {
                carry += buffer[j] << 8;
                buffer[j] = (byte)(carry % 58);
                carry /= 58;
            }

            while (carry > 0)
            {
                buffer[length++] = (byte)(carry % 58);
                carry /= 58;
            }
        }

        // Skip leading zeros in buffer (these are trailing zeros in the reverse result)
        int leadingZerosBuffer = 0;
        while (leadingZerosBuffer < length && buffer[leadingZerosBuffer] == 0)
        {
            leadingZerosBuffer++; // Shouldn't happen with this logic, but good for safety
        }

        // Construct the string
        char[] result = new char[zeros + length];

        // 1. Add '1' for each leading zero byte
        for (int i = 0; i < zeros; i++)
        {
            result[i] = CharacterSet[0];
        }

        // 2. Add the encoded characters (reverse the buffer)
        for (int i = 0; i < length; i++)
        {
            result[zeros + length - 1 - i] = CharacterSet[buffer[i]];
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
        int size = data.Length * 733 / 1000 + 1; // log(58) / log(256) is approx 0.732
        byte[] buffer = new byte[size];
        int length = 0;

        // Count leading '1's
        int zeros = 0;
        while (zeros < data.Length && data[zeros] == CharacterSet[0])
        {
            zeros++;
        }

        for (int i = zeros; i < data.Length; i++)
        {
            char c = data[i];
            int digit = CharacterSet.IndexOf(c);
            if (digit == -1)
            {
                throw new FormatException($"Invalid character '{c}' at position {i}");
            }

            int carry = digit;
            for (int j = 0; j < length; j++)
            {
                carry += buffer[j] * 58;
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
        int outputLength = zeros + length;
        byte[] result = new byte[outputLength];

        // 1. Leading zeros are already 0 in the new array

        // 2. Copy the decoded bytes in reverse order
        for (int i = 0; i < length; i++)
        {
            result[outputLength - 1 - i] = buffer[i];
        }

        return result;
    }
}