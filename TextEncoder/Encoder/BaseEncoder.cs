using System.Text;

namespace TextEncoder.Encoder;

public abstract class BaseEncoder : ITextEncoder
{
    private readonly Encoding _encoding = new UTF8Encoding(true, true);

    /// <inheritdoc />
    public string Encode(string text)
    {
        return this.ToBase(_encoding.GetBytes(text));
    }

    /// <inheritdoc />
    public string Decode(string data)
    {
        byte[] decoded = this.FromBase(data);
        return _encoding.GetString(decoded, 0, decoded.Length);
    }

    /// <inheritdoc />
    public abstract string ToBase(byte[] data);

    /// <inheritdoc />
    public abstract byte[] FromBase(string data);
}