using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.World.Items.Materials;

record PreciousMetal : Metal, IPreciousMetal
{
    public static PreciousMetal Gold { get; }

    static PreciousMetal()
    {
        Gold = new(MetalType.Gold);
    }

    private PreciousMetal(MetalType metalType) : base(metalType) { }
}