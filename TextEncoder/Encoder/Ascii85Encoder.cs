using System;
using System.IO;
using System.Text;

namespace TextEncoder.Encoder;

public class Ascii85Encoder : BaseEncoder
{
    public static readonly Ascii85Encoder Original = new();
    
    private Ascii85Encoder()
    {
        // Standard ASCII 85 verwendet einen festen Zeichensatz ('!' bis 'u')
    }

    /// <inheritdoc />
    public override string ToBase(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return string.Empty;
        }

        // Faktor ~1.25, + Sicherheitsmarge
        StringBuilder sb = new StringBuilder(data.Length * 5 / 4);
        int count = 0;
        uint tuple = 0;

        foreach (byte b in data)
        {
            if (count >= 3)
            {
                tuple |= b;
                if (tuple == 0)
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
            encoded[i] = (char)((tuple % 85) + 33);
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
                // Whitespace ignorieren (entsprechend Spezifikation)
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }

                // 'z' steht für 4 Null-Bytes
                if (c == 'z' && count == 0)
                {
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    ms.WriteByte(0);
                    continue;
                }

                if (c < '!' || c > 'u')
                {
                    throw new ArgumentException($"Ungültiges Zeichen im Ascii85 String: {c}");
                }

                tuple = tuple * 85 + (uint)(c - 33);
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
                    throw new ArgumentException("Ascii85 Block darf nicht nur aus einem Zeichen bestehen.");
                }

                // Padding simulieren ('u' entspricht Wert 84)
                int padding = 5 - count;
                for (int i = 0; i < padding; i++)
                {
                    tuple = tuple * 85 + 84;
                }

                // Wir schreiben (count - 1) Bytes
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