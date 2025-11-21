using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Enums;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Items.Components;

/// <summary>
/// Component for an item entity that can perform melee attacks.
/// </summary>
internal class MeleeAttackComponent(Dictionary<MeleeAttackType, IAttackDamage> attacks) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IMeleeAttack
{
    public IReadOnlyDictionary<MeleeAttackType, IAttackDamage> Attacks { get; } = attacks;
}