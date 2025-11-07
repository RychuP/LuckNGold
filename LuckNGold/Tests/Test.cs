using LuckNGold.Visuals.Screens;
using LuckNGold.Visuals.Windows;
using LuckNGold.World.Monsters;

namespace LuckNGold.Tests;

internal class Test : SimpleSurface
{
    //ColoredGlyph _rectAppearance = new(Color.CornflowerBlue, Color.Black, 178);
    //ShapeParameters _shapeParams = ShapeParameters.CreateBorder(appearance);

    public Test()
    {
        var player = MonsterFactory.Player();
        var preview = new CharacterPreview(player)
        {
            Position = (2, 2)
        };
        Children.Add(preview);

        var equipment = new EquipmentWindow()
        {
            Position = (20, 2)
        };
        Children.Add(equipment);

        Font = Game.Instance.Fonts["race-human-base-pale"];
        FontSize *= 4;
        
        int fontColumns = Font.Image.Width / Font.GlyphWidth;
        for (int y = 1; y < 5; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                int index = Point.ToIndex(x, y, fontColumns);
                Surface.SetGlyph(x, y + 1, index);
            }
        }
    }
}