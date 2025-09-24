namespace LuckNGold.Tests;

internal class Test : SimpleSurface
{
    public Test()
    {
        Rectangle one = new(1, 1, 8, 8);
        var appearance = new ColoredGlyph(Color.CornflowerBlue, Color.Black, 178);
        var shapeParams = ShapeParameters.CreateBorder(appearance);
        Surface.DrawBox(one, shapeParams);

        Rectangle two = one.Expand(-1, -2);
        appearance = new ColoredGlyph(Color.LightGreen, Color.Black, 177);
        shapeParams = ShapeParameters.CreateBorder(appearance);
        Surface.DrawBox(two, shapeParams);
    }
}