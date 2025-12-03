using System.Collections.Generic;

namespace TextEncoder.Encoder;

internal abstract class CharacterMapping : ICharacterMapping
{
    protected CharacterMapping(char[] characters, char? paddingChar = null)
    {
        this.Characters = characters;
        for (byte i = 0; i < characters.Length; i++)
        {
            this.CharValues[characters[i]] = i;
        }

        this.PaddingChar = paddingChar;
    }

    /// <inheritdoc />
    public char[] Characters { get; }

    /// <inheritdoc />
    public Dictionary<char, byte> CharValues { get; } = new();

    /// <inheritdoc />
    public char? PaddingChar { get; }
}