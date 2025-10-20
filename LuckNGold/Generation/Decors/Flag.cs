using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Decors;

record Flag : Decor
{
    public Gemstone Gemstone { get; init; }

    public Flag(Point position, Gemstone gemstone) : base(position)
    {
        Gemstone = gemstone;
    }
}