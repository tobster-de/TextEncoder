namespace TextEncoder.Encoder;

/// <summary>
/// Like Base64 but excludes non-alphanumeric characters (+ and /) and pairs of characters that often
/// look ambiguous when rendered: zero (0) and capitol-O (O), and capital-I (I) and lowercase-L (l).
/// </summary>
public class Base58Encoder : BaseEncoder
{
	public const string CharacterSet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

	/// <inheritdoc />
	public override string ToBase(byte[] data)
	{
		return string.Empty;
	}

	/// <inheritdoc />
	public override byte[] FromBase(string data)
	{
		return [];
	}
}

