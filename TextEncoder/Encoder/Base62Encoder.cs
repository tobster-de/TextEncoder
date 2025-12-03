namespace TextEncoder.Encoder;

/// <summary>
///	Like Base64 but encodes data as the 62 letters and digits of ASCII –
/// capital letters A-Z, lower case letters a-z and digits 0–9.
/// </summary>
public class Base62Encoder : Base64Encoder
{
    public static readonly Base62Encoder Instance = new();

    private class Base62Mapping() : CharacterMapping(CharacterSet.ToCharArray())
    {
        private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    }

    private Base62Encoder() : base(new Base62Mapping())
    {
    }
}