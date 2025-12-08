using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using TextEncoder.Encoder;

namespace TextEncoder;

/// <summary>
///	This class is an immutable representation of a Base64 encoding.
/// </summary>
[Serializable]
public sealed class Base64 : ISerializable
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
    ///	Returns the encoding of the Base64 value.
    /// </summary>
    public Base64Format Format { get; }

    private Base64(IEnumerable<byte> bytes, string value, Base64Format format)
    {
        _bytes = bytes.ToArray();
        this.Format = format;
        this.Value = value;
    }

    /// <summary>
    ///	Constructs a Base64 value from a byte array.
    /// </summary>
    /// <param name="bytes">Source bytes for the Base64 value.</param>
    /// <param name="format">Specifies the Base64 encoding format.</param>
    public Base64(IEnumerable<byte> bytes, Base64Format format = Base64Format.Default)
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
        if (obj is not Base64 asBase64)
        {
            return false;
        }

        return this.Format == asBase64.Format
               && this.Value.Equals(asBase64.Value);
    }

    /// <summary>
    ///	Returns a string that represents the Base64 value.
    /// </summary>
    /// <returns>A string that represents the Base64 value.</returns>
    public override string ToString()
    {
        return this.Value;
    }

    /// <summary>
    ///	Return the hash value of the Base64 value.
    /// </summary>
    /// <returns>A hash code for the current Base64 value.</returns>
    public override int GetHashCode()
    {
        return this.Value.GetHashCode();
    }

    /// <summary>
    ///	Parses a Base64 formated string to a Base64 object.
    /// </summary>
    /// <param name="value">Base64 formated string.</param>
    /// <param name="format">Specify the Base64 encoding format.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns>A Base64 representation of the Base64 formated string.</returns>
    public static Base64 Parse(string value, Base64Format format = Base64Format.Default)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        byte[] raw = EncoderFactory.GetEncoder(format).FromBase(value);
        return new Base64(raw, value, format);
    }

    /// <summary>
    ///	Tries to parse a Base64 formated string to a Base64 object.
    /// </summary>
    /// <param name="value">Base64 formated string.</param>
    /// <param name="base64">Return Base64 representation of the Base64 formatted string.</param>
    /// <param name="format">Specify the Base64 encoding format.</param>
    /// <returns>True if Parse of string was successful.</returns>
    public static bool TryParse(string value, out Base64? base64, Base64Format format = Base64Format.Default)
    {
        try
        {
            base64 = Parse(value, format);
            return true;
        }
        catch (Exception)
        {
            base64 = null;
            return false;
        }
    }

    #region Serializable

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("F", this.Format);
        info.AddValue("V", this.Value);
    }

    private Base64(SerializationInfo info, StreamingContext context)
    {
        this.Format = (Base64Format)info.GetValue("F", typeof(Base64Format));
        this.Value = (string)info.GetValue("V", typeof(string));
        _bytes = EncoderFactory.GetEncoder(this.Format).FromBase(this.Value);
    }

    #endregion Serializable
}