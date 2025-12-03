namespace TextEncoder.Encoder;

public interface ITextEncoder
{
    /// <summary>
    /// Encodes the provided UTF-8 encoded text into string for transmission etc.
    /// </summary>
    /// <param name="text">UTF-8 encoded text that will be encoded</param>
    /// <returns>Encoded data string</returns>
    string Encode(string text);

    /// <summary>
    /// Decodes provided data string into a UTF-8 encoded text
    /// </summary>
    /// <param name="data">Encoded data string</param>
    /// <returns>UTF-8 encoded text</returns>
    string Decode(string data);

    /// <summary>
    /// Encodes the byte data into string for transmission etc.
    /// </summary>
    /// <param name="data">Byte data to encode</param>
    /// <returns>Encoded string</returns>
    string ToBase(byte[] data);

    /// <summary>
    /// Decodes the byte data from string
    /// </summary>
    /// <param name="data">Encoded string to decode</param>
    /// <returns>Decoded byte data</returns>
    byte[] FromBase(string data);
}