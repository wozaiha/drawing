using System;
using ImGuiNET;
using Una.Drawing;

namespace ExamplePlugin.Tests;

public class StretchTest : ITest
{
    public string Name => "Stretch";

    private readonly Node _node = new() {
        Id = "Menu",
        Style = new() {
            Flow            = Flow.Vertical,
            BackgroundColor = new(0xFF212021),
            StrokeColor     = new(0xFF4F4F4F),
            StrokeWidth     = 1,
            BorderRadius    = 4,
            Gap             = 4,
            Padding         = new(4),
        },
        ChildNodes = [
            CreateItem("Item1", "Item 1"),
            CreateItem("Item2", "Another Item 2"),
            CreateItem("Item3", "Some other Item 3"),
            CreateItem("Item4", "Item 4"),
            CreateItem("Item5", "Short"),
        ]
    };

    private double _time;

    public void Render()
    {
        _time += 1;

        if (_time == 200) {
            _node.QuerySelector("Item4")!.NodeValue = "Now this text is much longer!";
        }

        if (_time == 400) {
            _node.QuerySelector("Item4")!.NodeValue = "Yes";
            _time                                   = 0;
        }

        _node.Render(ImGui.GetBackgroundDrawList(), new(100, 100));
    }

    private static Node CreateItem(string id, string label)
    {
        return new() {
            Id        = id,
            NodeValue = label,
            Style = new() {
                Stretch         = true,
                Size            = new(0, 32),
                Padding         = new(0, 8),
                TextAlign       = Anchor.MiddleCenter,
                BackgroundColor = new(0x80008080),
                BorderRadius    = 4,
            }
        };
    }
}
