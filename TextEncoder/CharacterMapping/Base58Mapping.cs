namespace TextEncoder.CharacterMapping;

/// <summary>
/// This character mapping excludes non-alphanumeric characters (+ and /) and pairs of characters that often
/// look ambiguous when rendered with Base64: zero (0) and capital-O (O), and capital-I (I) and lowercase-L (l).
/// </summary>
internal class Base58Mapping : CharacterMapping
{
    private static Base58Mapping? _instance;

    internal static Base58Mapping Instance => _instance ??= new Base58Mapping();

    private const string CharacterSet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

    private Base58Mapping() : base(CharacterSet.ToCharArray())
    {
    }
}