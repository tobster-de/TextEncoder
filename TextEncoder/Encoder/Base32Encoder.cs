using System;

namespace TextEncoder.Encoder;

/// <summary>
/// Base32 utilizes 4 bit segments
/// </summary>
public class Base32Encoder : BaseEncoder
{
	//The RFC 4648 Base32 alphabet
	public static readonly Base32Encoder Rfc4648     = new Base32Encoder("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567", true);
	public static readonly Base32Encoder ExtendedHex = new Base32Encoder("0123456789ABCDEFGHIJKLMNOPQRSTUV", true);
	public static readonly Base32Encoder ZBase32     = new Base32Encoder("ybndrfg8ejkmcpqxot1uwisza345h769", false);

	public static Base32Encoder GetEncoder(Base32Format format)
	{
		switch (format)
		{
			case Base32Format.RFC4648:
				return Rfc4648;
			case Base32Format.ZBase32:
				return ZBase32;
			//case Base32Format.Crockfords: //TODO


			case Base32Format.ExtendedHex:
				return ExtendedHex;
			default:
				throw new ArgumentOutOfRangeException(nameof(format), format, null);
		}
	}

	private Base32Encoder(string characterSet, bool usePadding)
	{
		this.CharacterSet = characterSet;
		this.UsePadding = usePadding;
	}

	/// <inheritdoc />
	public string CharacterSet { get; }

	/// <inheritdoc />
	public bool UsePadding { get; }

	/// <inheritdoc />
	public char PaddingChar => '=';

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