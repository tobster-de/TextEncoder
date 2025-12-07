using System.Linq;

namespace TextEncoder.Encoder;

internal class Ascii85Mapping : CharacterMapping
{
    public static readonly Ascii85Mapping Original = new Ascii85Mapping();
    public static readonly Ascii85Mapping ZeroMq 
        = new Ascii85Mapping("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.-:+=^!/*?&<>()[]{}@%$#");
    
    private Ascii85Mapping(string characters) : base(characters.ToCharArray())
    {
    }

    private Ascii85Mapping() : base(OriginalCharset)
    {
    }

    // Standard ASCII 85 uses a fixed character set ('!' to 'u').
    private static char[] OriginalCharset => Enumerable.Range('!', 'u').Select(i => (char)i).ToArray();
}