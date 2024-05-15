/* Una.Drawing                                                 ____ ___
 *   A declarative drawing library for FFXIV.                 |    |   \____ _____        ____                _
 *                                                            |    |   /    \\__  \      |    \ ___ ___ _ _ _|_|___ ___
 * By Una. Licensed under AGPL-3.                             |    |  |   |  \/ __ \_    |  |  |  _| .'| | | | |   | . |
 * https://github.com/una-xiv/drawing                         |______/|___|  (____  / [] |____/|_| |__,|_____|_|_|_|_  |
 * ----------------------------------------------------------------------- \/ --- \/ ----------------------------- |__*/

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
            BackgroundGradient = GradientColor.Vertical(new(0xFF212021), new(0xFF443911), 6),
            StrokeWidth     = 1,
            StrokeInset     = 5,
            BorderRadius    = 9,
            RoundedCorners  = RoundedCorners.TopLeft | RoundedCorners.TopRight,
            Gap             = 18,
            Padding         = new(16),
            ShadowSize      = new(64, 64, 0, 64),
            ShadowInset     = 2,
            ShadowOffset    = new(0, 8),
            IsAntialiased   = true,
        },
        ChildNodes = [
            CreateItem("Item1", "JA: グッデイワールド", 1),
            CreateItem("Item2", "CN: 好日子世界", 2),
            CreateItem("Item3", "KR: 굿데이월드", 3),
            CreateItem("Item4", "Item 4", 4),
            CreateItem("Item5", "Short", 5),
            CreateItem("Item6", "Foobar", 6 ),
            CreateItem("Item7", "Another one with very large text!", 7),
            CreateItem("Item8", null, 8)
        ]
    };

    private double _time;

    public StretchTest()
    {
        Color.AssignByName("stretch-test", 0xFF402070);

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

        if (_time == 300) {
            // _node.QuerySelector("Item4")!.NodeValue       = "Now this text is much much much much much longer!";
            // _node.QuerySelector("Item6")!.Style.IsVisible = true;
            Color.AssignByName("stretch-test", 0xFF407070);

            _node.QuerySelector("Item6")!.SortIndex = 1;
        }

        if (_time == 600) {
            // _node.QuerySelector("Item4")!.NodeValue       = "Yes";
            // _node.QuerySelector("Item6")!.Style.IsVisible = false;
            _time = 0;
            Color.AssignByName("stretch-test", 0xFF404050);

            _node.QuerySelector("Item6")!.SortIndex = 6;
        }

        _node.Render(ImGui.GetBackgroundDrawList(), new(500, 100));
    }

    private static Node CreateItem(string id, object? label, int sortIndex)
    {
        return new() {
            Id        = id,
            NodeValue = label,
            SortIndex = sortIndex,
            Style = new() {
                Stretch         = true,
                Size            = new(0, 32),
                Padding         = new(0, 8),
                Font            = 1,
                FontSize        = 14,
                TextAlign       = Anchor.MiddleCenter,
                BackgroundColor = new("stretch-test"),
                BorderRadius    = 4,
                TextShadowColor = new(0xFF000000),
                TextShadowSize  = 2,
                OutlineColor    = new(0xFF000000),
                OutlineSize     = 1,
                IsAntialiased   = false,
            }
        };
    }
}
