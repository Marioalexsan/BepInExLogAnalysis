namespace BepinexLogAnalysis.Test;

public class LogAnalyzerTests
{
    private readonly LogAnalyzer _logAnalyzer = new(new LogAnalyzerOptions()
    {
        RuleLists = [.. BundledRules.All.Values]
    });

    [Fact(DisplayName = "Drops lines that are way too long")]
    public async Task CheckLineDrop()
    {
        using var input = new MemoryStream();
        await using var writer = new StreamWriter(input, leaveOpen: true);

        var superLongLine = new string('a', 999999);
        
        await writer.WriteLineAsync(superLongLine);
        await writer.WriteLineAsync("[Message:   BepInEx] BepInEx 5.4.23.4 - ATLYSS (10/10/2025 22:22:29)");
        await writer.WriteLineAsync(superLongLine);
        await writer.WriteLineAsync(superLongLine);
        await writer.WriteLineAsync("[Info   :   BepInEx] Running under Unity v2022.3.62.7762112");
        await writer.WriteLineAsync(superLongLine);
        await writer.WriteLineAsync("[Info   :   BepInEx] Loading [Tanuki.Atlyss.Bootstrap 2.4.0]");
        await writer.WriteLineAsync(superLongLine);
        await writer.WriteLineAsync(superLongLine);

        input.Position = 0;
        
        var report = await _logAnalyzer.ProcessLogAsync(input, TestContext.Current.CancellationToken);
        
        Assert.False(report.LikelyInvalid);
        Assert.Equal(6, report.ProcessingErrors.Count);
        Assert.All(report.ProcessingErrors, error => Assert.Contains("is over the length limit", error));
    }
}