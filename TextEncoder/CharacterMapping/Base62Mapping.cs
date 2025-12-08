namespace TextEncoder.CharacterMapping;

/// <summary>
/// This character mapping corresponds to Base64 but
/// excludes '+' and '/' and does no padding.
/// </summary>
internal class Base62Mapping : CharacterMapping
{
    public static readonly Base62Mapping Instance = new();

    private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private Base62Mapping() : base(CharacterSet.ToCharArray())
    {
    }
}