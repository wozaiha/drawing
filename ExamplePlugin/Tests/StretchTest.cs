using Dalamud.Game.Text.SeStringHandling;
using ImGuiNET;
using Una.Drawing;

namespace ExamplePlugin.Tests;

public class StretchTest : ITest
{
    public string Name => "Stretch";

    private readonly Node _node = new() {
        Id = "Menu",
        Style = new() {
            Anchor          = Anchor.TopCenter,
            Flow            = Flow.Vertical,
            BackgroundColor = new(0xFF212021),
            StrokeColor     = new(0xFF4F4F4F),
            StrokeWidth     = 1,
            BorderRadius    = 4,
            Gap             = 4,
            Padding         = new(4),
            Opacity         = 0.5f,
        },
        ChildNodes = [
            CreateItem("Item1", "JA: グッデイワールド"),
            CreateItem("Item2", "CN: 好日子世界"),
            CreateItem("Item3", "KR: 굿데이월드"),
            CreateItem("Item4", "Item 4"),
            CreateItem("Item5", "Short"),
            CreateItem("Item6", "Foobar"),
            CreateItem("Item7", "Another one with very large text!"),
            CreateItem("Item8", null)
        ]
    };

    private double _time;

    public StretchTest()
    {
        _node.QuerySelector("Item6")!.Style.IsVisible = false;

        SeString str = new SeStringBuilder()
            .AddUiForeground(28)
            .AddText("SeString test with ")
            .AddUiForegroundOff()
            .AddIcon(BitmapFontIcon.IslandSanctuary)
            .AddText(" a very nice icon. Neat stuff!")
            .Build();

        _node.QuerySelector("Item8")!.NodeValue = str;
    }

    public void Render()
    {
        _time += 1;

        if (_time == 600) {
            _node.QuerySelector("Item4")!.NodeValue       = "Now this text is much much much much much longer!";
            _node.QuerySelector("Item6")!.Style.IsVisible = true;
        }

        if (_time == 1200) {
            _node.QuerySelector("Item4")!.NodeValue       = "Yes";
            _node.QuerySelector("Item6")!.Style.IsVisible = false;
            _time                                         = 0;
        }

        _node.Render(ImGui.GetBackgroundDrawList(), new(500, 100));
    }

    private static Node CreateItem(string id, object? label)
    {
        return new() {
            Id        = id,
            NodeValue = label,
            Style = new() {
                Stretch         = true,
                Size            = new(0, 32),
                Padding         = new(0, 8),
                Font            = 1,
                FontSize        = 14,
                TextAlign       = Anchor.MiddleCenter,
                BackgroundColor = new(0x80008080),
                BorderRadius    = 4,
                TextShadowColor = new(0xFF000000),
                TextShadowSize  = 2,
                OutlineColor    = new(0xFF000000),
                OutlineSize     = 1,
            }
        };
    }
}
