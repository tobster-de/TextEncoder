using System.Linq;

namespace TextEncoder.CharacterMapping;

internal class Ascii85Mapping : CharacterMapping
{
    private static Ascii85Mapping? _original;
    private static Ascii85Mapping? _zeroMq;

    /// <summary>
    /// Standard ASCII 85 uses a fixed character set (ASCII '!' to 'u').
    /// </summary>
    public static Ascii85Mapping Original
        => _original ??= new Ascii85Mapping(Enumerable.Range('!', 'u').Select(i => (char)i).ToArray());

    /// <summary>
    /// Z85 alternate character set, more suitable for embedding binary data in source code, URLs, or JSON without requiring escaping.
    /// </summary>
    public static Ascii85Mapping ZeroMq
        => _zeroMq ??= new Ascii85Mapping("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#".ToCharArray());

    private Ascii85Mapping(char[] characters) : base(characters)
    {
    }
}