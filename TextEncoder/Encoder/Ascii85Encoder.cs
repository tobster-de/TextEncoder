using System;
using System.IO;
using System.Text;
using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

/// <summary>
/// Ascii85, also called Base85, is a binary-to-text encoding using five ASCII characters to represent four
/// bytes of binary data (making the encoded size 1⁄4 larger than the original, assuming eight bits per ASCII
/// character). This is more efficient than Base64.
/// </summary>
public class Ascii85Encoder : BaseEncoder
{
    private static Ascii85Encoder? _original;
    private static Ascii85Encoder? _zeroMq;

    /// <summary>
    /// Standard Ascii85 uses a fixed character set (ASCII '!' to 'u').
    /// </summary>
    public static Ascii85Encoder Original => _original ??= new Ascii85Encoder(Ascii85Mapping.Original, useCompression: true, ignoreWhitespace: true);

    /// <summary>
    /// Z85 is a variant of Ascii85 developed by the ZeroMQ project more suitable for embedding
    /// binary data in source code, URLs, or JSON without requiring escaping.
    /// </summary>
    public static Ascii85Encoder ZeroMq => _zeroMq ??= new Ascii85Encoder(Ascii85Mapping.ZeroMq, useCompression: false, ignoreWhitespace: false);

    private readonly ICharacterMapping _mapping;
    private readonly byte[] _characterMap;
    private readonly bool _useCompression;
    private readonly bool _ignoreWhitespace;

    private Ascii85Encoder(Ascii85Mapping mapping, bool useCompression, bool ignoreWhitespace)
    {
        _mapping = mapping;
        _characterMap = CreateCharacterMap(mapping.CharValues);
        _useCompression = useCompression;
        _ignoreWhitespace = ignoreWhitespace;
    }

    /// <inheritdoc />
    public override string ToBase(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return string.Empty;
        }

        // Factor ~1.25, + safety margin
        StringBuilder sb = new StringBuilder(data.Length * 5 / 4);
        int count = 0;
        uint tuple = 0;

        foreach (byte b in data)
        {
            if (count >= 3)
            {
                tuple |= b;
                
                // Only use compression 'z' if enabled (default Ascii85)
                if (_useCompression && tuple == 0)
                {
                    sb.Append('z');
                }
                else
                {
                    EncodeBlock(sb, tuple, 5);
                }
                tuple = 0;
                count = 0;
            }
            else
            {
                tuple |= (uint)(b << (24 - (count * 8)));
                count++;
            }
        }

        if (count > 0)
        {
            EncodeBlock(sb, tuple, count + 1);
        }

        return sb.ToString();
    }

    private void EncodeBlock(StringBuilder sb, uint tuple, int charsToWrite)
    {
        char[] encoded = new char[5];
        for (int i = 4; i >= 0; i--)
        {
            // Use characters from the mapping
            encoded[i] = _mapping.Characters[(int)(tuple % 85)];
            tuple /= 85;
        }
        for (int i = 0; i < charsToWrite; i++)
        {
            sb.Append(encoded[i]);
        }
    }

    /// <inheritdoc />
    public override byte[] FromBase(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            return new byte[0];
        }

        using (MemoryStream ms = new MemoryStream())
        {
            uint tuple = 0;
            int count = 0;

            foreach (char c in data)
            {
                // Ignore whitespace (only if enabled)
                if (_ignoreWhitespace && char.IsWhiteSpace(c))
                {
                    continue;
                }

                // 'z' stands for 4 zero bytes (only when activated)
                if (_useCompression && c == 'z' && count == 0)
                {
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    continue;
                }

                // Get value from decoding map
                byte val = (c < _characterMap.Length) ? _characterMap[c] : (byte)0xFF;

                if (val == 0xFF)
                {
                    throw new FormatException($"Invalid character in Ascii85 string: {c}");
                }

                tuple = tuple * 85 + val;
                count++;

                if (count == 5)
                {
                    ms.WriteByte((byte)(tuple >> 24));
                    ms.WriteByte((byte)(tuple >> 16));
                    ms.WriteByte((byte)(tuple >> 8));
                    ms.WriteByte((byte)tuple);
                    tuple = 0;
                    count = 0;
                }
            }

            if (count > 0)
            {
                if (count == 1)
                {
                    throw new FormatException("An ASCII85 block must not consist of only one character.");
                }

                // Simulate padding ('u' or value 84 corresponds to the last character in the alphabet)
                int padding = 5 - count;
                for (int i = 0; i < padding; i++)
                {
                    tuple = tuple * 85 + 84;
                }

                // We write (count - 1) bytes
                int bytesToWrite = count - 1;
                for (int i = 0; i < bytesToWrite; i++)
                {
                    ms.WriteByte((byte)(tuple >> (24 - (i * 8))));
                }
            }

            return ms.ToArray();
        }
    }
}