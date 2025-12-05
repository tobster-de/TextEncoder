namespace TextEncoder;

public enum Base32Format
{
    /// <summary>
    ///		Represents encoding according to RFC4648 https://tools.ietf.org/html/rfc4648.
    /// </summary>
    RFC4648 = 0,
    /// <summary>
    ///		Represents z-base-32 a Base32 encoding designed to be easier for human use and more compact.
    ///     It includes 1, 8 and 9 but excludes l, v and 2. It also permutes the alphabet so that the easier
    ///     characters are the ones that occur more frequently. It compactly encodes bitstrings whose length in
    ///     bits is not a multiple of 8, and omits trailing padding characters.
    /// </summary>
    ZBase32 = 1,
    /// <summary>
    ///		Represents Crockfords Base32 alternative design for Base32
    ///     Douglas Crockford proposed using additional characters for a checksum.
    ///     It excludes the letters I, L, and O to avoid confusion with digits.
    ///     It also excludes the letter U to reduce the likelihood of accidental obscenity.
    /// </summary>
    Crockford = 2,
    /// <summary>
    ///		Triacontakaidecimal is another alternative design for Base 32, which extends hexadecimal in a more natural
    ///     way and was first proposed by Christian Lanctot, a programmer working at Sage software, in a letter to
    ///     Dr. Dobb's magazine in March 1999[8] as a proposed solution for solving the Y2K bug and referred to as "Double Hex".
    ///     This version was described in RFC 2938 https://tools.ietf.org/html/rfc2938 under the name "Base-32".
    /// </summary>
    ExtendedHex = 3
}