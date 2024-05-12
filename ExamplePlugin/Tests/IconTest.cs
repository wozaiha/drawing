using ImGuiNET;
using Una.Drawing;

namespace ExamplePlugin.Tests;

public class IconTest : ITest
{
    public string Name => "Icons";

    private readonly Node _node = new() {
        Style = new() {
            Flow = Flow.Vertical,
            Gap  = 4,
        },
        ChildNodes = [
            CreateBlock(62101),
            CreateBlock(62102),
            CreateBlock(62103),
            CreateBlock(62104),
            CreateBlock(62105),
            CreateBlock(62106),
            CreateBlock(62107),
            CreateBlock(62108),
            CreateBlock(62109),
            CreateBlock(62110),
        ]
    };

    public IconTest()
    {
        Stylesheet.SetClassRule(
            "icon-row",
            new() {
                Anchor          = Anchor.TopLeft,
                Size            = new(0, 40),
                Padding         = new(4),
                BorderRadius    = 12,
                BackgroundColor = new(0x80232223),
                StrokeColor     = new(0xFF6A6A6A),
                StrokeWidth     = 1,
                Gap             = 10,
            }
        );

        Stylesheet.SetClassRule(
            "icon",
            new() {
                Size            = new(32, 32),
                BackgroundColor = new(0x80232223),
                BorderColor     = new(new(0xFFC0C0C0)),
                BorderWidth     = new(2),
                BorderInset     = 1,
                BorderRadius    = 8,
                IconRounding    = 16,
                IconInset       = new(4),
                IconGrayscale   = true,
            }
        );

        Stylesheet.SetClassRule(
            "icon-desc",
            new() {
                Size         = new(0, 32),
                TextAlign    = Anchor.MiddleLeft,
                WordWrap     = false,
                Font         = "default",
                FontSize     = 14,
                OutlineColor = new(0xFF000000),
                OutlineSize  = 1,
                Padding      = new() { Right = 4 },
            }
        );
    }

    public void Render()
    {
        _node.Render(ImGui.GetBackgroundDrawList(), new(200, 100));
    }

    private static Node CreateBlock(uint iconId)
    {
        Node box = new() {
            ClassList = ["icon-row"],
            ChildNodes = [
                new() {
                    ClassList = ["icon"],
                    Style = new() {
                        IconId = iconId,
                    },
                },
                new() {
                    ClassList = ["icon-desc"],
                    NodeValue = $"Icon #{iconId}"
                },
            ]
        };

        box.OnMouseEnter += _ => {
            box.QuerySelector(".icon")!.Style.IconGrayscale = false;
            box.QuerySelector(".icon")!.Style.IconInset     = new(-4);
        };

        box.OnMouseLeave += _ => {
            box.QuerySelector(".icon")!.Style.IconGrayscale = true;
            box.QuerySelector(".icon")!.Style.IconInset     = new(4);
        };

        return box;
    }
}
