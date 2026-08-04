# BepInExLogAnalysis

A simple library for analyzing log files for BepInEx games (mainly ATLYSS).

Supports the following features:
- Output a list of errors, warnings, etc. based on severity
  - Prioritizes errors and warnings by default
  - Lines can also be increased or decreased in severity based on regex patterns
- Transform data from logs based on log patterns and output them in common formats
  - Tables
  - Groups of 2 properties (key, value)
  - Groups of 3 properties (first key, second key, value)
  - Arbitrary fields

# Usage

## Creating an analyzer

```csharp

# Create an analyzer using all bundled rules
LogAnalyzer analyzer = new LogAnalyzer(new LogAnalyzerOptions()
{
    RuleLists = [.. BundledRules.All.Values]
});

# ... or with specific rules
analyzer = new LogAnalyzer(new LogAnalyzerOptions()
{
    RuleLists = [.. BundledRules.Core, .. BundledRules.Atlyss]
});

# ... or with your own rules
var ruleList = JsonSerializer.Deserialize<LogRuleList>("/path/to/file");
analyzer = new LogAnalyzer(new LogAnalyzerOptions()
{
    RuleLists = [ruleList]
});
```

## Analyzing logs from a stream

```csharp

using Stream logFile = File.OpenRead("LogOutput.log");
LogReport report = analyzer.ProcessLogAsync(logFile, cancellationToken);

Console.WriteLine(report.Game);
Console.WriteLine(report.BepInExVersion);
Console.WriteLine(report.GameVersion);

```

## Rendering report content extracted using rule lists

```csharp
LogReport report = analyzer.ProcessLogAsync(logFile, cancellationToken);

foreach (var (sectionName, section) in report.Content)
{
    writer.WriteLine($"--- {sectionName} ---");
    writer.WriteLine();
    
    switch (section)
    {
        case Dictionary<string, string> group2:
            RenderGroup2(writer, group2);
            break;
        case Dictionary<string, Dictionary<string, string>> group3:
            RenderGroup3(writer, group3);
            break;
        case List<List<string>> table:
            RenderTable(writer, table);
            break;
        case List<string> list:
            RenderList(writer, list);
            break;
    }
    
    writer.WriteLine();
}
```

## Specify custom rules

You can specify and load custom rules according to the [C# schema](./BepInExLogAnalysis/LogRule.cs):

```json
{
  "name": "scoring",
  "rules": [
    {
      "description": "Highlight important log lines",
      "sourceFilter": ["SourceNameOfAmazingMod"],
      "contentPattern": "*super amazing mod crashed! message:*",
      "scoring": {
        "severity": 15
      }
    },
    {
      "description": "Ignore false positives or unactionable log lines",
      "sourceFilter": ["SourceNameOfModThatSucks"],
      "contentPattern": "*mod that sucks ass crashed! probably because not updated! message:*",
      "scoring": {
        "ignore": true
      }
    },
    {
      "contentPattern": "Quest \"(?<quest>.*)\" not found!",
      "sourceFilter": ["CustomQuests"],
      "scoring": {
        "ignore": true
      },
      "transform": {
        "section": "Plugins/Custom Quests",
        "rule": "table(quest)"
      }
    }
  ]
}
```