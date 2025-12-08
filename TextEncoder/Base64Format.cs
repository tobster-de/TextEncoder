namespace TextEncoder;

/// <summary>
/// Represents different Base64 formats
/// </summary>
public enum Base64Format
{
    /// <summary>
    /// Default Base64 format
    /// </summary>
    Default,
    /// <summary>
    /// Base64 without using padding at the end.
    /// </summary>
    WithoutPadding,
    /// <summary>
    /// Like the default Base64 format but using URL safe characters.
    /// </summary>
    UrlEncoding,
    /// <summary>
    /// Like the default Base64 format but using XML compatible characters.
    /// </summary>
    XmlEncoding,
    /// <summary>
    /// Like the default Base64 format but using RegEx compatible characters.
    /// </summary>
    RegExEncoding,
    /// <summary>
    /// Like the default Base64 format but using file system safe characters.
    /// </summary>
    FileEncoding
}