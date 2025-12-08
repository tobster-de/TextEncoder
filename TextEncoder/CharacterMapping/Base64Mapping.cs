namespace TextEncoder.CharacterMapping;

internal class Base64Mapping : CharacterMapping
{
    private static Base64Mapping? _default;
    private static Base64Mapping? _defaultNoPadding;
    private static Base64Mapping? _urlEncoding;
    private static Base64Mapping? _xmlEncoding;
    private static Base64Mapping? _regExEncoding;
    private static Base64Mapping? _fileEncoding;

    internal static Base64Mapping Default => _default ??= new Base64Mapping('+', '/', '=');
    internal static Base64Mapping DefaultNoPadding => _defaultNoPadding ??= new Base64Mapping('+', '/');
    internal static Base64Mapping UrlEncoding => _urlEncoding ??= new Base64Mapping('-', '_');
    internal static Base64Mapping XmlEncoding => _xmlEncoding ??= new Base64Mapping('_', ':');
    internal static Base64Mapping RegExEncoding => _regExEncoding ??= new Base64Mapping('!', '-');
    internal static Base64Mapping FileEncoding => _fileEncoding ??= new Base64Mapping('+', '-');

    private const string CharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private Base64Mapping(char char63, char char64, char? paddingChar = null)
        : base((CharacterSet + char63 + char64).ToCharArray(), paddingChar)
    {
    }
}