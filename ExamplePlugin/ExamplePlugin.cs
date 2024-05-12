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

    private string _activeTest = "Stretch";

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

        Stylesheet.SetClassRule(
            "button",
            new() {
                Size               = new(0, 26),
                Padding            = new(0, 6),
                BackgroundColor    = new(0xC01A1A1A),
                BorderColor        = new(new(0xFF787A7A)),
                BorderWidth        = new(1),
                BorderRadius       = 5,
                BorderInset        = 2,
                BackgroundGradient = GradientColor.Vertical(new(0xC02F2A2A), null, 5),
                TextAlign          = Anchor.MiddleCenter
            }
        );

        Stylesheet.SetClassRule(
            "button-hover",
            new() {
                Color              = new(0xFF101010),
                BackgroundColor    = new(0xC0EAEAEA),
                BackgroundGradient = GradientColor.Vertical(new(0xC02FFFFF), null, 5),
            }
        );
    }

    public void Dispose()
    {
        _plugin.UiBuilder.Draw -= OnDraw;
        _renderer.Dispose();
    }

    private void OnDraw()
    {
        ImGui.SetNextWindowSize(new(600, 84), ImGuiCond.Always);
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

        ImGui.SetCursorPos(new(10, 56));

        foreach (var test in _tests) {
            if (ImGui.Button(test.Key)) {
                _activeTest = test.Key;
            }

            ImGui.SameLine();
        }

        ImGui.End();

        if (_tests.TryGetValue(_activeTest, out var activeTest)) {
            activeTest.Render();
        }
    }
}
