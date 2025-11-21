using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.World.Items.Materials;

record Leather : Material, ILeather
{
    public LeatherType LeatherType { get; init; }

    public static Leather BovineHide { get; }
    public static Leather BeastHide { get; }
    public static Leather RotheHide { get; }
    public static Leather ShadowLeather { get; }
    public static Leather DragonHide { get; }

    static Leather()
    {
        BovineHide = new(LeatherType.BovineHide);
        BeastHide = new(LeatherType.BeastHide);
        RotheHide = new(LeatherType.RotheHide);
        ShadowLeather = new(LeatherType.ShadowLeather);
        DragonHide = new(LeatherType.DragonHide);
    }

    private Leather(LeatherType leatherType) : base(MaterialType.Leather)
    {
        LeatherType = leatherType;
    }

    public override string ToString() => $"{LeatherType}";
}