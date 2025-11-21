using LuckNGold.Config;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Items.Materials.Interfaces;

namespace LuckNGold.World.Items.Materials;

record Gemstone : Material, IGemstone
{
    public GemstoneType GemstoneType { get; init; }
    public Color Color => Theme.GemstoneColors[GemstoneType];

    public static Gemstone Onyx { get; }
    public static Gemstone Amber { get; }
    public static Gemstone Ruby { get; }
    public static Gemstone Emerald { get; }
    public static Gemstone Diamond { get; }

    public static IGemstone[] List;

    static Gemstone()
    {
        Onyx = new(GemstoneType.Onyx);
        Amber = new(GemstoneType.Amber);
        Ruby = new(GemstoneType.Ruby);
        Emerald = new(GemstoneType.Emerald);
        Diamond = new(GemstoneType.Diamond);

        List = [Onyx, Amber, Ruby, Emerald, Diamond];
    }

    private Gemstone(GemstoneType gemstoneType) : base(MaterialType.Stone)
    {
        GemstoneType = gemstoneType;
    }

    public static IGemstone Get(GemstoneType type) =>
        Array.Find(List, g => g.GemstoneType == type) ??
        throw new InvalidOperationException("Gemstone could not be found.");

    public override string ToString() => $"{GemstoneType}";
}