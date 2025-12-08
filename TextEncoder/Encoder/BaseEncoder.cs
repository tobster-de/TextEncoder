using System;
using System.Collections.Generic;
using System.Text;

namespace TextEncoder.Encoder;

/// <summary>
/// Base class for all text encoders.
/// </summary>
public abstract class BaseEncoder : ITextEncoder
{
    private readonly Encoding _encoding = new UTF8Encoding(true, true);

    /// <summary>
    /// Creates the reverse character mapping for the given character values.
    /// </summary>
    protected static byte[] CreateCharacterMap(Dictionary<char, byte> characterValues)
    {
        byte[] characterMap = new byte[255];

#if NETSTANDARD
        Array.Fill(characterMap, (byte)0xFF);
#elif NETFRAMEWORK
        for (int i = 0; i < characterMap.Length; i++)
        {
            characterMap[i] = (byte)0xFF;
        }
#else
#error "Target framework not supported"
#endif

        foreach (KeyValuePair<char, byte> valuePair in characterValues)
        {
            characterMap[(byte)valuePair.Key] = valuePair.Value;
        }

        return characterMap;
    }
    
    /// <inheritdoc />
    public string Encode(string text)
    {
        return this.ToBase(_encoding.GetBytes(text));
    }

    /// <inheritdoc />
    public string Decode(string data)
    {
        byte[] decoded = this.FromBase(data);
        return _encoding.GetString(decoded, 0, decoded.Length);
    }

    /// <inheritdoc />
    public abstract string ToBase(byte[] data);

    /// <inheritdoc />
    public abstract byte[] FromBase(string data);
}