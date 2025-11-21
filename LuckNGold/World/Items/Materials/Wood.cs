using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.World.Items.Materials;

record Wood : Material, IWood
{
    public WoodType WoodType { get; init; }

    public static Wood Darkwood { get; }
    public static Wood Ironwood { get; }
    public static Wood Cinderwood { get; }
    public static Wood Dreadwood { get; }
    public static Wood Petrifiedwood { get; }

    static Wood()
    {
        Darkwood = new(WoodType.Darkwood);
        Ironwood = new(WoodType.Ironwood);
        Cinderwood = new(WoodType.Cinderwood);
        Dreadwood = new(WoodType.Dreadwood);
        Petrifiedwood = new(WoodType.Petrifiedwood);
    }

    private Wood(WoodType woodType) : base(MaterialType.Wood)
    {
        WoodType = woodType;
    }

    public override string ToString() => $"{WoodType}";
}