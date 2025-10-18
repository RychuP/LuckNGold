using LuckNGold.Generation.Items;
using LuckNGold.World.Items.Enums;

namespace LuckNGold.Generation.Furniture;

record Lock(Difficulty Difficulty)
{
    /// <summary>
    /// Material of a matching <see cref="Key"/>.
    /// </summary>
    public Gemstone Gemstone => (Gemstone) Difficulty;
}