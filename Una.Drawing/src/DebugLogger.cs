using Dalamud.Plugin.Services;

namespace Una.Drawing;

public static class DebugLogger
{
    public static IPluginLog? Writer { get; set; }

    public static void Log(string message)
    {
        Writer?.Debug(message);
    }
}
