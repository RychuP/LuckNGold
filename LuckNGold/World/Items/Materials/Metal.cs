using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.World.Items.Materials;

record Metal : Material, IMetal
{
    public MetalType MetalType { get; init; }

    public static Metal MoonSteel { get; }

    static Metal()
    {
        MoonSteel = new(MetalType.MoonSteel);
    }

    protected Metal(MetalType metalType) : base(MaterialType.Metal)
    {
        MetalType = metalType;
    }

    public override string ToString() => $"{MetalType}";
}