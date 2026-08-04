using BepinexLogAnalysis;
using BepinexLogAnalysis.Test;

namespace BepInExLogAnalysis.Test.LogRules;

public class BaseRuleList
{
    [Fact(DisplayName = "Rule set loads correctly")]
    public void Load()
    {
        Assert.NotEmpty(BundledRules.Base);
        Assert.All(BundledRules.Base, ruleList => Assert.NotEmpty(ruleList.Rules));
    }

    [Fact(DisplayName = "Detects game version and BepInEx vesion")]
    public async Task DetectMetadata()
    {
        await using var logFile = TestUtils.GetTestLog("CoreRuleList");

        var logAnalyzer = new LogAnalyzer(new LogAnalyzerOptions
        {
            RuleLists = [.. BundledRules.Base]
        });

        var report = await logAnalyzer.ProcessLogAsync(logFile, TestContext.Current.CancellationToken);
        
        Assert.Equal("ATLYSS", report.Game);
        Assert.Equal("5.4.23.5", report.BepInExVersion);
    }

    [Theory(DisplayName = "Game version detection works correctly")]
    [InlineData("VersionDetect-ATLYSS-HB", "12026.a3")]
    public async Task DetectsGameVersion(string id, string expectedGame)
    {
        await using var logFile = TestUtils.GetTestLog(id);

        var logAnalyzer = new LogAnalyzer(new LogAnalyzerOptions
        {
            RuleLists = [.. BundledRules.Base]
        });

        var report = await logAnalyzer.ProcessLogAsync(logFile, TestContext.Current.CancellationToken);
        
        Assert.Equal(expectedGame, report.GameVersion);
    }
}