using BenchmarkDotNet.Running;

namespace BepInExLogAnalysis.Benchmark;

public static class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<StressTest>();
        
        Console.WriteLine("==============================================================");
        Console.WriteLine($"# Log sample size (MiB): {Data.BigLog.Length / 1048576}");
        Console.WriteLine("==============================================================");
    }
}