
using TextEncoder.CharacterMapping;
using TextEncoder.Radix;

namespace TextEncoder.Encoder;

/// <summary>
/// This represents a radix encoder using 52 characters.
/// </summary>
public class Radix52Encoder : RadixEncoder
{
    private static Radix52Encoder? _withoutDigits;
    private static Radix52Encoder? _withoutVowels;

    /// <summary>
    /// Only includes upper and lower case characters of the alphabet.
    /// </summary>
    public static Radix52Encoder WithoutDigits => _withoutDigits ??= new Radix52Encoder(Base52Mapping.WithoutDigits);

    /// <summary>
    /// Includes numerical digits but excludes vowels
    /// </summary>
    public static Radix52Encoder WithoutVowels => _withoutVowels ??= new Radix52Encoder(Base52Mapping.WithoutVowels);

    private Radix52Encoder(Base52Mapping characterMapping) : base(characterMapping.Characters)
    {
    }
}