namespace TextEncoder.CharacterMapping;

internal interface ICharacterMapping
{
    char[] Characters { get; }

    System.Collections.Generic.Dictionary<char, byte> CharValues { get; }

    /// <summary>
    /// When padding is used this contains the padding character, otherwise it's null
    /// </summary>
    char? PaddingChar { get; }
}