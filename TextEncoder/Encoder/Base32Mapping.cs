namespace TextEncoder.Encoder;

internal class Base32Mapping : CharacterMapping
{
    public static readonly Base32Mapping Rfc4648     = new Base32Mapping("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", '=');
    public static readonly Base32Mapping ExtendedHex = new Base32Mapping("0123456789ABCDEFGHIJKLMNOPQRSTUV", '=');
    public static readonly Base32Mapping ZBase32     = new Base32Mapping("ybndrfg8ejkmcpqxot1uwisza345h769");
    public static readonly Base32Mapping Crockford   = new CrockFordMapping();

    /// <inheritdoc />
    protected Base32Mapping(string characters, char? paddingChar = null) : base(characters.ToCharArray(), paddingChar)
    {
    }
}

/// <remarks>
/// https://www.crockford.com/base32.html
/// </remarks>
internal class CrockFordMapping : Base32Mapping
{
    private const string CharacterSet = "0123456789ABCDEFGHJKMNPQRSTVWXYZ";

    /// <inheritdoc />
    public CrockFordMapping() : base(CharacterSet)
    {
        this.CharValues['o'] = 0;
        this.CharValues['O'] = 0;
        this.CharValues['i'] = 1;
        this.CharValues['I'] = 1;
        this.CharValues['l'] = 1;
        this.CharValues['L'] = 1;

        // lower case characters decoding
        for (byte i = 0; i < 26; i++)
        {
            this.CharValues[(char)('a' + i)] = (byte)(i + 10);
        }
    }
}