using LuckNGold.World.Monsters.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for monster entities that have attributes affecting their actions.
/// </summary>
/// <param name="strength">Strength of the monster entity.</param>
/// <param name="dexterity">Dexterity of the monster entity.</param>
/// <param name="wisdom">Wisdom of the monster entity.</param>
internal class StatsComponent(int strength, int dexterity, int wisdom) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IStats
{
    public int Strength { get; } = strength;
    public int Dexterity { get; } = dexterity;
    public int Wisdom { get; } = wisdom;
}