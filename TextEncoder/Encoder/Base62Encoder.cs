using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

/// <summary>
///	Like Base64 but encodes data as the 62 letters and digits of ASCII –
/// capital letters A-Z, lower case letters a-z and digits 0–9.
/// </summary>
public class Base62Encoder : BaseEncoderWithCustomCharset
{
    private static Base62Encoder? _instance;

    /// <summary>
    /// Returns the singleton instance of this encoder.
    /// </summary>
    public static Base62Encoder Instance => _instance ??= new Base62Encoder();

    private Base62Encoder() : base(Base62Mapping.Instance)
    {
    }
}