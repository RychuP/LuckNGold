using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that can respond to bumps.
/// </summary>
internal class BumpableComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IBumpable
{
    public void OnBumped(RogueLikeEntity source)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.AllComponents.GetFirstOrDefault<ICombatant>() is ICombatant parentCombatant &&
            source.AllComponents.GetFirstOrDefault<ICombatant>() is ICombatant sourceCombatant)
        {
            var attack = sourceCombatant.GetAttack();
            var defence = parentCombatant.GetDefence();
        }
    }
}