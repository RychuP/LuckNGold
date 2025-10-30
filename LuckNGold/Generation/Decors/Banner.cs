using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Decors;

record Banner : Decor
{
    public Gemstone Gemstone { get; init; }

    public Banner(Point position, Gemstone gemstone) : base(position)
    {
        Gemstone = gemstone;
    }
}