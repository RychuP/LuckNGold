using LuckNGold.Generation.Items.Tools;
using LuckNGold.World.Common.Enums;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Furnitures;

record Lock(Difficulty Difficulty)
{
    /// <summary>
    /// Material of a matching <see cref="Key"/>.
    /// </summary>
    public GemstoneType Gemstone => (GemstoneType) Difficulty;
}