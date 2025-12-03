namespace TextEncoder.Encoder;

internal class Base64Mapping : CharacterMapping
{
    internal static readonly Base64Mapping Default = new('+', '/', '=');
    internal static readonly Base64Mapping DefaultNoPadding = new('+', '/');
    internal static readonly Base64Mapping UrlEncoding = new('-', '_');
    internal static readonly Base64Mapping XmlEncoding = new('_', ':');
    internal static readonly Base64Mapping RegExEncoding = new('!', '-');
    internal static readonly Base64Mapping FileEncoding = new('+', '-');

    private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private Base64Mapping(char char63, char char64, char? paddingChar = null)
        : base((CharacterSet + char63 + char64).ToCharArray(), paddingChar)
    {
    }
}