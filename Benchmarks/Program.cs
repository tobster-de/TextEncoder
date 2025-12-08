using System;
using BenchmarkDotNet.Running;

namespace Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
#if DEBUG
        throw new NotSupportedException("Performance comparison only useful for non debug builds.");
#else
        BenchmarkRunner.Run<Base64Benchmarks>();
#endif
    }
}