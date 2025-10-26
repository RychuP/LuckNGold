using LuckNGold.Generation.Items;
using LuckNGold.World.Common.Enums;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Furnitures;

record Lock(Difficulty Difficulty)
{
    /// <summary>
    /// Material of a matching <see cref="Key"/>.
    /// </summary>
    public Gemstone Gemstone => (Gemstone) Difficulty;
}