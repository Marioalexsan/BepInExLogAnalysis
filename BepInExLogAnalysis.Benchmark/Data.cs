using System.Text;

namespace BepInExLogAnalysis.Benchmark;

public static class Data
{
    private static readonly string BigLogData = string.Join('\n', [
        """
        [Message:   BepInEx] BepInEx 5.4.23.5 - ATLYSS (5/13/2026 5:22:10 AM)
        [Info   :   BepInEx] Running under Unity v2022.3.62.7762112
        [Info   :   BepInEx] CLR runtime version: 4.0.30319.42000
        [Info   :   BepInEx] Supports SRE: True
        [Info   :   BepInEx] System platform: Bits64, Windows
        [Info   :   BepInEx] Loaded 1 patcher method from [BepInEx.Preloader 5.4.23.5]
        [Info   :   BepInEx] 1 patcher plugin loaded
        [Info   :   BepInEx] Patching [UnityEngine.CoreModule] with [BepInEx.Chainloader]
        [Info   :   BepInEx] 1 plugins to load
        [Info   :   BepInEx] Loading [Homebrewery 4.7.30]
        [Info   :Homebrewery] Waking up at 2026-07-28 10:16:19 PM UTC... Game Version is: 12026.a3
        [Info   :Homebrewery] Dear me, where did all the bumpscosity go? Quite unnerving in here with all of it gone.

        [Info   :Homebrewery] AtlyssGLTF available: False.
        [Info   :Homebrewery] Info:
        Mod's folder name: Catman232-Homebrewery
        Plugins path: [redacted]\BepInEx\plugins
        ContentPacks path: [redacted]\BepInEx\config\ContentPacks
        [Info   :Homebrewery] Calling _harmony.PatchAll
        [Info   :Homebrewery] Checking HomebreweryFiles and constructing objects!
        [Info   :Homebrewery] Finding other content folders and constructing objects!
        """,
        .. Enumerable.Repeat(
        """
        [Error  : Unity Log] NullReferenceException: Object reference not set to an instance of an object
        Stack trace:
        Homebrewery.Code.Content.Parts.TailPartFolder.TailProperties () (at ./Code/Content/Parts/TailPartFolder.cs:107)
        Homebrewery.Code.Content.Parts.TailPartFolder.SetupVanillaTailParts () (at ./Code/Content/Parts/TailPartFolder.cs:178)
        Homebrewery.HB.Awake () (at ./HB.cs:311)
        UnityEngine.GameObject:AddComponent()
        Homebrewery.BepInExPlugin:Awake() (at ./HB.cs:48)
        UnityEngine.GameObject:AddComponent(Type)
        BepInEx.Bootstrap.Chainloader:Start()
        UnityEngine.Application:.cctor()
        SettingsManager:Awake()
        """,
        100000
        )
    ]);
    
    public static readonly byte[] BigLog = Encoding.UTF8.GetBytes(BigLogData);
}