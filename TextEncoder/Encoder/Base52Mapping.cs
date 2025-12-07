namespace TextEncoder.Encoder;

public partial class Base52Encoder
{
    private class Base52Mapping : CharacterMapping
    {
        internal static readonly Base52Mapping WithoutDigits = new("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
        internal static readonly Base52Mapping WithoutVowels = new("0123456789BCDFGHJKLMNPQRSTVWXYZbcdfghjklmnpqrstvwxyz");

        private Base52Mapping(string characterSet) : base(characterSet.ToCharArray(), '=')
        {
        }
    }
}