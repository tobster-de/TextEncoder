
using TextEncoder.CharacterMapping;
using TextEncoder.Radix;

namespace TextEncoder.Encoder;

/// <summary>
/// This represents a radix encoder using 62 characters and digits.
/// </summary>
public class Radix62Encoder : RadixEncoder
{
    private static Radix62Encoder? _instance;

    /// <summary>
    /// Includes the ten digits and upper and lower case characters of the alphabet.
    /// </summary>
    public static Radix62Encoder Instance => _instance ??= new Radix62Encoder(Base62Mapping.Instance);

    private Radix62Encoder(Base62Mapping characterMapping) : base(characterMapping.Characters)
    {
    }
}