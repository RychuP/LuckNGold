using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Defences.Interfaces;

namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It can fight and defend.
/// </summary>
internal interface ICombatant
{
    IAttack GetAttack();
    IDefence GetDefence();
    void Resolve(IAttack attack, IDefence defence, IHealth health);
}