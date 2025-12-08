using TextEncoder.CharacterMapping;

namespace TextEncoder.Encoder;

public class Base52Encoder : BaseEncoderWithCustomCharset
{
    private static Base52Encoder? _withoutDigits;
    private static Base52Encoder? _withoutVowels;

    /// <summary>
    /// Only includes upper and lower case characters of the alphabet.
    /// </summary>
    public static Base52Encoder WithoutDigits => _withoutDigits ??= new Base52Encoder(Base52Mapping.WithoutDigits);

    /// <summary>
    /// Includes numerical digits but excludes vowels
    /// </summary>
    public static Base52Encoder WithoutVowels => _withoutVowels ??= new Base52Encoder(Base52Mapping.WithoutVowels);

    private Base52Encoder(Base52Mapping characterMapping) : base(characterMapping)
    {
    }
}