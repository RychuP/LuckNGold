using LuckNGold.World.Items.Materials;

namespace LuckNGold.Generation.Items.Bodywears.Clothings;

record LinenClothing : Clothing
{
    public LinenClothing(Point position) : base(position, Fabric.Linen) { }
}