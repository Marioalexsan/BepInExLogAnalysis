using BenchmarkDotNet.Attributes;
using BepinexLogAnalysis;

namespace BepInExLogAnalysis.Benchmark;

public class StressTest
{
    [Benchmark]
    public async Task Atlyss()
    {
        using var memoryStream = new MemoryStream(Data.BigLog);

        var analyzer = new LogAnalyzer(new LogAnalyzerOptions()
        {
            RuleLists = [.. BundledRules.All.Values]
        });

        var result = await analyzer.ProcessLogAsync(memoryStream);
    }
}