using System.Linq;

namespace TextEncoder.CharacterMapping;

internal class Ascii85Mapping : CharacterMapping
{
    /// <summary>
    /// Standard ASCII 85 uses a fixed character set (ASCII '!' to 'u').
    /// </summary>
    public static readonly Ascii85Mapping Original
        = new Ascii85Mapping(Enumerable.Range('!', 'u').Select(i => (char)i).ToArray());

    /// <summary>
    /// Z85 alternate character set
    /// </summary>
    public static readonly Ascii85Mapping ZeroMq
        = new Ascii85Mapping("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#".ToCharArray());

    private Ascii85Mapping(char[] characters) : base(characters)
    {
    }
}