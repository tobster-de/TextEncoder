using System;
using TextEncoder.Encoder;

namespace TextEncoderTests;

public abstract class GenericTestBase<TEncoder> where TEncoder: ITextEncoder
{
    public const int Count = 21;

    // ReSharper disable once StaticMemberInGenericType
    // it's not desired to share date between different generics
    public static byte[][] RandomData { get; } = new byte[Count][];

    public TEncoder? Subject { get; protected set; }

    static GenericTestBase()
    {
        var r = new Random(DateTime.Now.Day * Count);
        for (int i = 0; i < Count; i++)
        {
            RandomData[i] = new byte[i];

            for (int j = 0; j < i; j++)
            {
                RandomData[i][j] = (byte)r.Next(255);
            }
        }
    }
}