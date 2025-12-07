namespace TextEncoder.Encoder;

public class Base52Encoder : BaseEncoderWithCustomCharset
{
    /// <summary>
    /// Only includes upper and lower case characters of the alphabet.
    /// </summary>
    public static readonly Base52Encoder WithoutDigits = new Base52Encoder(Base52Mapping.WithoutDigits);

    /// <summary>
    /// Includes numerical digits but excludes vowels
    /// </summary>
    public static readonly Base52Encoder WithoutVowels = new Base52Encoder(Base52Mapping.WithoutVowels);

    private Base52Encoder(Base52Mapping characterMapping) : base(characterMapping)
    {
    }
}