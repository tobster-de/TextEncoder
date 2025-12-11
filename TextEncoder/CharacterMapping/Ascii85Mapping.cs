using System;
using System.Linq;

namespace TextEncoder.CharacterMapping;

internal class Ascii85Mapping : CharacterMapping
{
    private static Ascii85Mapping? _original;
    private static Ascii85Mapping? _zeroMq;

    /// <summary>
    /// Standard ASCII 85 uses a fixed character set (ASCII '!' to 'u').
    /// </summary>
    public static Ascii85Mapping Original => _original ??= new Ascii85Mapping(Range('!', 'u').ToCharArray());

    /// <summary>
    /// Z85 alternate character set, more suitable for embedding binary data in source code, URLs, or JSON without requiring escaping.
    /// </summary>
    public static Ascii85Mapping ZeroMq
        => _zeroMq ??= new Ascii85Mapping((Range('0', '9') + Range('a', 'z') + Range('A', 'Z') + ".-:+=^!/*?&<>()[]{}@%$#").ToCharArray());

    private Ascii85Mapping(char[] characters) : base(characters)
    {
    }

    private static string Range(char startChar, char endChar)
    {
        //if (startChar > endChar)
        //    throw new ArgumentException(nameof(startChar), $"{nameof(startChar)} must be smaller than {nameof(endChar)}.");

        int length = (endChar - startChar) + 1;

        char[] asciiArray = new char[length];

        for (int i = 0; i < length; i++)
        {
            asciiArray[i] = (char)(startChar + i);
        }

        return string.Concat(asciiArray);
    }
}