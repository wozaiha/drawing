using Dalamud.Plugin.Services;

namespace Una.Drawing;

public static class Logger
{
    public static IPluginLog? Writer { get; set; }

    public static void Log(string message)
    {
        Writer?.Debug(message);
    }
}
