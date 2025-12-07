using System;
using System.Text;

namespace TextEncoder.Radix;

/// <summary>
/// Radix encoding is a method of encoding numbers in a given radix (base).
/// In a positional numeral system, the radix is the number of unique digits, including the digit zero,
/// used to represent numbers. For example, for the decimal system the radix is ten,
/// because it uses the ten digits from 0 through 9.
/// </summary>
/// <remarks>
/// The algorithm is based on two core mathematical concepts:
/// Encoding: Repeated division and modulo operations (remainder calculation) to determine the digits in the target system.
/// Decoding: Polynomial calculation (Horner's scheme) to convert the string back into a number.
/// </remarks>
public class RadixEncoder
{
    // The character set used for encoding (0-9, A-Z)
    // This supports bases up to 36.
    private static string CharacterSet => "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private readonly int _radix;

    /// <summary>
    /// Initializes a new instance of the RadixEncoder class with a specific base.
    /// </summary>
    /// <param name="radix">The base system to use (must be between 2 and 36).</param>
    public RadixEncoder(int radix)
    {
        if (radix < 2 || radix > CharacterSet.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(radix), 
                $"Radix must be between 2 and {CharacterSet.Length}.");
        }
        _radix = radix;
    }

    /// <summary>
    /// Converts a decimal number (base 10) to a string representation in the defined radix.
    /// </summary>
    /// <param name="value">The decimal value to encode.</param>
    /// <returns>The encoded string.</returns>
    /// <remarks>
    /// To convert a number to another system (e.g. hexadecimal / base 16), we use a loop:
    /// We take the remainder of the division by the base (number % radix).
    /// This remainder is the index for the character in the character set.
    /// We divide the number by the base (number / radix) to get to the next digit.
    /// This is repeated until the number is 0.
    /// </remarks>
    public string Encode(long value)
    {
        // Handle the zero case immediately
        if (value == 0) return "0";

        // Handle negative numbers
        bool isNegative = value < 0;
        
        // Use absolute value for calculation. 
        // Note: Math.Abs(long.MinValue) throws OverflowException, 
        // so real-world production code might need unsigned handling for that specific edge case.
        long targetValue = Math.Abs(value);

        StringBuilder buffer = new StringBuilder();

        // Algorithm: Repeatedly divide by radix and prepend the remainder
        while (targetValue > 0)
        {
            long remainder = targetValue % _radix;
            buffer.Insert(0, CharacterSet[(int)remainder]);
            targetValue /= _radix;
        }

        // Restore negative sign if necessary
        if (isNegative)
        {
            buffer.Insert(0, '-');
        }

        return buffer.ToString();
    }

    /// <summary>
    /// Converts a string representation in the defined radix back to a decimal number.
    /// </summary>
    /// <param name="input">The encoded string.</param>
    /// <returns>The decoded decimal value.</returns>
    /// <remarks>
    /// We iterate through the string from left to right.
    /// We multiply the current result by the base (result * radix).
    /// This ‘shifts’ the previous digits one place to the left.
    /// We add the value of the current digit.
    /// </remarks>
    public long Decode(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Input cannot be null or empty.", nameof(input));

        long result = 0;
        int startIndex = 0;
        bool isNegative = false;

        // Check for negative sign
        if (input[0] == '-')
        {
            isNegative = true;
            startIndex = 1;
        }

        // Algorithm: Iterate through chars, multiply result by radix and add current digit value
        for (int i = startIndex; i < input.Length; i++)
        {
            char c = input[i];
            
            // Find the numeric value of the character case-insensitively
            int charValue = CharacterSet.IndexOf(char.ToUpper(c));

            // Validation: Character must exist in set and be valid for the current radix
            if (charValue == -1 || charValue >= _radix)
            {
                throw new FormatException($"The character '{c}' is invalid for radix {_radix}.");
            }

            // Horner's method: result = result * base + value
            result = result * _radix + charValue;
        }

        return isNegative ? -result : result;
    }
}