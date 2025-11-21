using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.World.Items.Materials;

record Fabric : Material, IFabric
{
    public FabricType FabricType { get; }

    public static Fabric Linen { get; }

    static Fabric()
    {
        Linen = new(FabricType.Linen);
    }

    private Fabric(FabricType fabricType) : base(MaterialType.Fabric)
    {
        FabricType = fabricType;
    }

    public override string ToString() => $"{FabricType}";
}