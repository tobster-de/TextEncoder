using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

/// <summary>
///	Like Base64 but encodes data as the 62 letters and digits of ASCII –
/// capital letters A-Z, lower case letters a-z and digits 0–9.
/// </summary>
public class Base62Encoder : BaseEncoderWithCustomCharset
{
    public static readonly Base62Encoder Instance = new();

    private Base62Encoder() : base(Base62Mapping.Instance)
    {
    }
}