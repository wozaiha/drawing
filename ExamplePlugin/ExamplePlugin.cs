using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using ExamplePlugin.Tests;
using ImGuiNET;
using Una.Drawing;

namespace ExamplePlugin;

public sealed class ExamplePlugin : IDalamudPlugin
{
    private readonly DalamudPluginInterface    _plugin;
    private readonly Renderer                  _renderer;
    private readonly Dictionary<string, ITest> _tests = [];

    private string _activeTest = "Basic";

    public ExamplePlugin(IPluginLog logger, DalamudPluginInterface plugin)
    {
        Logger.Writer = logger;
        _plugin       = plugin;
        _renderer     = Renderer.Create(plugin);

        // Node.DrawDebugInfo = true;

        var tests = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && t.IsAssignableTo(typeof(ITest)))
            .ToList();

        foreach (var test in tests) {
            var instance = (ITest)Activator.CreateInstance(test)!;
            _tests[instance.Name] = instance;
        }

        _plugin.UiBuilder.Draw += OnDraw;
    }

    public void Dispose()
    {
        _plugin.UiBuilder.Draw -= OnDraw;
        _renderer.Dispose();
    }

    private void OnDraw()
    {
        ImGui.SetNextWindowSize(new(200, 56), ImGuiCond.Always);
        ImGui.SetNextWindowPos(new(10, 10));
        ImGui.Begin("UnaDrawingTestSuite");

        if (ImGui.BeginCombo("Tests", _activeTest)) {
            if (ImGui.Selectable("None", _activeTest == "")) {
                _activeTest = "";
            }

            foreach (var test in _tests) {
                var selected = _activeTest == test.Key;
                if (ImGui.Selectable(test.Key, selected)) {
                    _activeTest = test.Key;
                }
            }

            ImGui.EndCombo();
        }

        ImGui.End();

        if (_tests.TryGetValue(_activeTest, out var activeTest)) {
            activeTest.Render();
        }
    }
}
