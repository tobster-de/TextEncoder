
using TextEncoder.CharacterMapping;
using TextEncoder.Radix;

namespace TextEncoder.Encoder;

/// <summary>
/// This represents a radix encoder using 62 characters and digits.
/// </summary>
public class Radix62Encoder : RadixEncoder
{
    /// <summary>
    /// Includes the ten digits and upper and lower case characters of the alphabet.
    /// </summary>
    public static readonly Radix62Encoder Instance = new Radix62Encoder(Base62Mapping.Instance);

    private Radix62Encoder(Base62Mapping characterMapping) : base(characterMapping.Characters)
    {
    }
}