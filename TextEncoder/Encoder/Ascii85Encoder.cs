using System;
using System.IO;
using System.Text;
using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

public class Ascii85Encoder : BaseEncoder
{
    public static readonly Ascii85Encoder Original = new Ascii85Encoder(Ascii85Mapping.Original, useCompression: true, ignoreWhitespace: true);
    public static readonly Ascii85Encoder ZeroMq = new Ascii85Encoder(Ascii85Mapping.ZeroMq, useCompression: false, ignoreWhitespace: false);

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
                    throw new ArgumentException($"Invalid character in Ascii85 string: {c}");
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
                    throw new ArgumentException("An ASCII85 block must not consist of only one character.");
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