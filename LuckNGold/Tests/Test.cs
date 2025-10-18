using LuckNGold.Visuals;

namespace LuckNGold.Tests;

internal class Test : SimpleSurface
{
    //ColoredGlyph _rectAppearance = new(Color.CornflowerBlue, Color.Black, 178);
    //ShapeParameters _shapeParams = ShapeParameters.CreateBorder(appearance);

    public Test()
    {
        int x = 0 / 2;
        Surface.Print(1, 1, x.ToString());
    }
}