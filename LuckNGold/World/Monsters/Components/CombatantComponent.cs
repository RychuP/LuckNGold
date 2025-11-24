using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for monster entities that can fight and defend.
/// </summary>
internal class CombatantComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ICombatant
{
    public IAttack GetAttack()
    {
        throw new NotImplementedException();
    }

    public IDefence GetDefence()
    {
        throw new NotImplementedException();
    }

    public void Resolve(IAttack attack, IDefence defence, IHealth health)
    {
        throw new NotImplementedException();
    }
}