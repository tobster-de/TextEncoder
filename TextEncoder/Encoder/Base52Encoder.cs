namespace TextEncoder.Encoder;

/// <summary>
/// Like Base64 but with a limited character set.
/// </summary>
public class Base52Encoder : Base64Encoder
{
	/// <summary>
	/// Only includes upper and lower case characters of the alphabet.
	/// </summary>
	public static readonly Base52Encoder WithoutDigits = new Base52Encoder(Base52Mapping.WithoutDigits);

	/// <summary>
	/// Includes numerical digits but excludes vowels
	/// </summary>
	public static readonly Base52Encoder WithoutVowels = new Base52Encoder(Base52Mapping.WithoutVowels);

	private class Base52Mapping : CharacterMapping
	{
		internal static readonly Base52Mapping WithoutDigits = new("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
		internal static readonly Base52Mapping WithoutVowels = new("0123456789BCDFGHJKLMNPQRSTVWXYZbcdfghjklmnpqrstvwxyz");

		private Base52Mapping(string characterSet) : base(characterSet.ToCharArray(), '=')
		{
		}
	}

	private Base52Encoder(Base52Mapping mapping) : base(mapping)
	{
	}
}