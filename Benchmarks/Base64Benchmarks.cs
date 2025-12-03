using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TextEncoder.Encoder;

namespace Benchmarks;

[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
public class Base64Benchmarks
{
    private readonly Base64Encoder _base64Encoder = Base64Encoder.Default;

    [Params(20, 50)] public int Length;

    [GlobalSetup]
    public void Setup()
    {
        var r = new Random(DateTime.Now.Day * Length);
        byte[] data = new byte[Length];
        for (int i = 0; i < Length; i++)
        {
            data[i] = (byte)r.Next(255);
        }

        Data = data;
        Text = Convert.ToBase64String(data);
    }

    private static byte[] Data { get; set; } = null!;

    private static string Text { get; set; } = null!;

    [Benchmark]
    public string ToBase64_UsingConvert()
    {
        return Convert.ToBase64String(Data);
    }

    [Benchmark]
    public string ToBase64_UsingBase64Encoder()
    {
        return _base64Encoder.ToBase(Data);
    }

    [Benchmark]
    public byte[] FromBase64_UsingConvert()
    {
        return Convert.FromBase64String(Text);
    }

    [Benchmark]
    public byte[] FromBase64_UsingBase64Encoder()
    {
        return _base64Encoder.FromBase(Text);
    }
}