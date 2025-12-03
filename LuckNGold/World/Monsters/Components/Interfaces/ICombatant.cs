using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Turns.Actions;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can fight and defend.
/// </summary>
internal interface ICombatant
{
    IAttack GetAttack();
    IDefence GetDefence();
    void Resolve(IAttack attack);
    AttackAction GetAttackAction(RogueLikeEntity target);
}