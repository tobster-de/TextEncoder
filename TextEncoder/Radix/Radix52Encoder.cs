
using TextEncoder.CharacterMapping;
using TextEncoder.Radix;

namespace TextEncoder.Encoder;

/// <summary>
/// This represents a radix encoder using 52 characters.
/// </summary>
public class Radix52Encoder : RadixEncoder
{
    /// <summary>
    /// Only includes upper and lower case characters of the alphabet.
    /// </summary>
    public static readonly Radix52Encoder WithoutDigits = new Radix52Encoder(Base52Mapping.WithoutDigits);

    /// <summary>
    /// Includes numerical digits but excludes vowels
    /// </summary>
    public static readonly Radix52Encoder WithoutVowels = new Radix52Encoder(Base52Mapping.WithoutVowels);

    private Radix52Encoder(Base52Mapping characterMapping) : base(characterMapping.Characters)
    {
    }
}