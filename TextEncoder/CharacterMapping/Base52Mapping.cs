namespace TextEncoder.CharacterMapping;

internal class Base52Mapping : CharacterMapping
{
    private static Base52Mapping? _withoutDigits;
    private static Base52Mapping? _withoutVowels;

    internal static Base52Mapping WithoutDigits => _withoutDigits ??= new Base52Mapping("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
    internal static Base52Mapping WithoutVowels => _withoutVowels ??= new Base52Mapping("0123456789BCDFGHJKLMNPQRSTVWXYZbcdfghjklmnpqrstvwxyz");

    private Base52Mapping(string characterSet) : base(characterSet.ToCharArray())
    {
    }
}