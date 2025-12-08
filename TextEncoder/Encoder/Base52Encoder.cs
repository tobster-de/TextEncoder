using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

/// <summary>
/// Text encoder using only 52 characters, which can be the lower and upper case letters of the alphabet
/// or the numerical digits 0 through 9 and all characters without the vowels.
/// </summary>
public class Base52Encoder : BaseEncoderWithCustomCharset
{
    private static Base52Encoder? _withoutDigits;
    private static Base52Encoder? _withoutVowels;

    /// <summary>
    /// Only includes upper and lower case characters of the alphabet.
    /// </summary>
    public static Base52Encoder WithoutDigits => _withoutDigits ??= new Base52Encoder(Base52Mapping.WithoutDigits);

    /// <summary>
    /// Includes numerical digits but excludes all vowels to avoid unwanted obscenity in the encoded text.
    /// </summary>
    public static Base52Encoder WithoutVowels => _withoutVowels ??= new Base52Encoder(Base52Mapping.WithoutVowels);

    private Base52Encoder(Base52Mapping characterMapping) : base(characterMapping)
    {
    }
}