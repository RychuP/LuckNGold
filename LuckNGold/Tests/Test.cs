using LuckNGold.World;

namespace LuckNGold.Tests;

internal class Test : SimpleSurface
{
    //ColoredGlyph _rectAppearance = new(Color.CornflowerBlue, Color.Black, 178);
    //ShapeParameters _shapeParams = ShapeParameters.CreateBorder(appearance);

    public Test()
    {
        var pos = (1, 1);
        var decor = new AnimatedRogueLikeEntity(pos);
        Surface.Print(1, 3, decor.IsSingleCell.ToString());
    }
}