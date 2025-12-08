namespace TextEncoder.CharacterMapping;

/// <summary>
/// This character mapping corresponds to Base64 but
/// excludes '+' and '/' and does no padding.
/// </summary>
internal class Base62Mapping : CharacterMapping
{
    private static Base62Mapping? _instance;

    public static Base62Mapping Instance => _instance ??= new Base62Mapping();

    private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private Base62Mapping() : base(CharacterSet.ToCharArray())
    {
    }
}