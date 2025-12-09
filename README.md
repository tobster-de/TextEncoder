# TextEncoder

[![Build Status](https://dev.azure.com/tkolb80/TextEncoder/_apis/build/status%2FCreate%20Package?branchName=main)](https://dev.azure.com/tkolb80/TextEncoder/_build/latest?definitionId=10&branchName=main)
![NuGet Version](https://img.shields.io/nuget/v/TextEncoder)
![NuGet Downloads](https://img.shields.io/nuget/dt/TextEncoder)

[`Install-Package TextEncoder`](https://www.nuget.org/packages/TextEncoder)

This library contains a collection of text encoder algorithms, such as Base64.

## Text encoding algorithms

**Base64**

The best known Base64 encoding is defined in RFC 4648. It is widely used in email and web applications. 
There is no real need for another implementation in .NET, because there is `Convert.ToBase64String`.
However, there are several variants of Base64, which Convert does not cover. These variants are supported
by this library and can be used to encode in special use cases, such as URLs, XML and RegEx patterns.

The implementation in this library might compete with the default implementation in .NET as effort is made to
optimize the encoding process. There are benchmarks available in the repository.

**Base32**

Base32 is a binary-to-text encoding that uses an alphabet of 32 digits, each of which represents a different 
combination of 5 bits ($2^5$). The question of notation, i.e. which characters to use to represent the 32 digits
and whether to pad the final encoding, is left to the application.

In this library, there are several character sets available:
- as described in RFC 4648: letters from 'A' to 'Z' and digits from '2' to '7', padding applied at the end
- Extended Hex aka Base32Hex: digits from '0' to '9', letters from 'A' to 'V', padding applied at the end
- ZBase32: Includes 1, 8 and 9 but excludes l, v, 0 and 2. It also permutes the alphabet so that the easier 
  characters are the ones that occur more frequently. Resulting in `ybndrfg8ejkmcpqxot1uwisza345h769`, without padding
- [Crockford's Base32](https://www.crockford.com/base32.html): the characters `0123456789ABCDEFGHJKMNPQRSTVWXYZ`, 
  i.e., excludes the letters I, L, and O to avoid confusion with digits. It also excludes the letter U. No padding is applied.

**Asci85, Z85**

Ascii85, also called Base85, is a binary-to-text encoding using five ASCII characters to represent four
bytes of binary data (making the encoded size 1⁄4 larger than the original, assuming eight bits per ASCII
character). This is more efficient than Base64.

Z85 is a variant of Ascii85 developed by the ZeroMQ project more suitable for embedding binary data in 
source code, URLs, or JSON without requiring escaping.

**Base62**

Excluding the characters '+' and '/' from the Base64 alphabet, Base62 uses the 62 upper case and lower case letters
as well as the 10 digits to encode binary data.

**Base58**

It is similar to Base64 but uses a different alphabet and fewer characters. It is more concerned with not confusing 
letters and digits that are looking similar. So Base58 retains the digit 1 and does not use the lower case letter 
l or the capital letter I. In mistaking the number 0 with the lower case letter o and the upper case letter O, Base58 
keeps the lower case letter o but does not use the digit 0 or the capital letter O.

Base58 is used for Bitcoin addresses, which contain 200 Bit (i.e., 25 Byte) data, but it can be used for any binary 
data when avoiding confusion between letters and digits is required.

**Base52**

Base52 encoding is available using a character set of 52 characters, consisting of the 26 lowercase letters (a-z) and 
26 uppercase letters (A-Z). Alternatively, there is a character set that excludes vowels to avoid offensive words when
encoding but uses the 10 digits.

## Number encoding algorithms

Although this library is not that concerned with the encoding of numbers, there are several number encoding 
algorithms available. Number encoding differs from text encoding in that it is not using arbitrary byte sequences  
but encoding bits of the numbers directly.

**Base2 through Base36**

These number encoding schemes use the digits 0 to 9 and the letters A to Z. The base (also known as latin 'radix') 
determines the number of unique characters used to represent the numbers. There is the `RadixEncoder` class that 
covers all of these bases (radizes).

**Base52, Radix52**

Base52 exploits using a larger alphabet to encode numbers, resulting in shorter encoding. As there is no commonly used
alphabet, the `Base52Encoder` alphabets are reused.

**Base62, Radix62**

Base62 uses even more characters. The used alphabet is taken from Base64 text encoding, but the characters '+' and '/'
are excluded.

## History

### 1.0.1

Just a documentation update (Readme)

### 1.0.0

Initial version

* Base64 (multiple)
* Base32 (multiple)

* Ascii85, Z85 (ZeroMq)

Encoders with limited character set
* Base52
* Base58
* Base62

Other elements
* Base64 and Base32 value types
* RadixEncoder

* Nuget
* Documentation