using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials;

namespace LuckNGold.Generation.Items.Tools;

record Key : Tool
{
    public Key(Point position, GemstoneType material) : base(position, Gemstone.Get(material)) { }
}