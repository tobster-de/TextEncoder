using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TextEncoder.Encoder;

namespace TextEncoder;

/// <summary>
///	This class is an immutable representation of a Base32 encoding.
/// </summary>
[Serializable]
public sealed class Base32 : ISerializable
{
    private byte[] _bytes;

    /// <summary>
    /// Returns raw byte data value presentation of this instance.
    /// </summary>
    public byte[] Raw => _bytes.ToArray();

    /// <summary>
    /// Returns the encoded value of this instance.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///	Returns the encoding of the Base32 value.
    /// </summary>
    public Base32Format Format { get; }

    private Base32(IEnumerable<byte> bytes, string value, Base32Format format)
    {
        _bytes = bytes.ToArray();
        this.Format = format;
        this.Value = value;
    }

    /// <summary>
    ///	Constructs a Base32 value from a byte array.
    /// </summary>
    /// <param name="bytes">Source bytes for the Base32 value.</param>
    /// <param name="format">Specifies the Base32 encoding format.</param>
    public Base32(IEnumerable<byte> bytes, Base32Format format = Base32Format.RFC4648)
    {
        _bytes = bytes.ToArray();
        this.Format = format;
        this.Value = EncoderFactory.GetEncoder(format).ToBase(this.Raw);
    }

    /// <summary>
    ///	Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Base32 asBase32)
        {
            return false;
        }

        return this.Format == asBase32.Format
               && this.Value.Equals(asBase32.Value);
    }

    /// <summary>
    ///	Returns a string that represents the Base32 value.
    /// </summary>
    /// <returns>A string that represents the Base32 value.</returns>
    public override string ToString()
    {
        return this.Value;
    }

    /// <summary>
    ///	Return the hash value of the Base32 value.
    /// </summary>
    /// <returns>A hash code for the current Base32 value.</returns>
    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }

    /// <summary>
    /// Parses a Base32 formated string to a Base32 object.
    /// </summary>
    /// <param name="value">Base32 formated string.</param>
    /// <param name="format">Specify the Base32 encoding format.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A Base32 representation of the Base32 formated string.</returns>
    public static Base32 Parse(string value, Base32Format format = Base32Format.RFC4648)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        byte[] raw = EncoderFactory.GetEncoder(format).FromBase(value);
        return new Base32(raw, value, format);
    }

    /// <summary>
    ///	Tries to parse a Base32 formated string to a Base32 object.
    /// </summary>
    /// <param name="value">Base32 formated string.</param>
    /// <param name="base32">Return Base32 representation of the Base32 formatted string.</param>
    /// <param name="format">Specify the Base32 encoding format.</param>
    /// <returns>True if Parse of string was successful.</returns>
    public static bool TryParse(string value, out Base32? base32, Base32Format format = Base32Format.RFC4648)
    {
        try
        {
            base32 = Parse(value, format);
            return true;
        }
        catch (Exception)
        {
            base32 = null;
            return false;
        }
    }

    #region Serializable

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("F", this.Format);
        info.AddValue("V", this.Value);
    }

    private Base32(SerializationInfo info, StreamingContext context)
    {
        this.Format = (Base32Format)info.GetValue("F", typeof(Base32Format));
        this.Value = (string)info.GetValue("V", typeof(string));
        _bytes = EncoderFactory.GetEncoder(this.Format).FromBase(this.Value);
    }

    #endregion Serializable
}