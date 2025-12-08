using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

/// <summary>
/// Like Base64 but excludes non-alphanumeric characters (+ and /) and pairs of characters that often
/// look ambiguous when rendered: zero (0) and capital-O (O), and capital-I (I) and lowercase-L (l).
/// </summary>
public class Base58Encoder : BaseEncoderWithCustomCharset
{
    private static Base58Encoder? _instance;

    public static Base58Encoder Instance => _instance ??= new Base58Encoder();

    private Base58Encoder() : base(Base58Mapping.Instance)
    {
    }
}