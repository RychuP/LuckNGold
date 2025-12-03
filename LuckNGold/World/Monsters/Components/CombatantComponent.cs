using GoRogue.Random;
using LuckNGold.Config;
using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Items.Damage;
using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Items.Defences;
using LuckNGold.World.Items.Defences.Interfaces;
using LuckNGold.World.Monsters.Components.Interfaces;
using LuckNGold.World.Turns.Actions;
using SadRogue.Integration;
using SadRogue.Integration.Components;
using ShaiRandom.Generators;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for monster entities that can fight and defend.
/// </summary>
internal class CombatantComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ICombatant
{
    /// <summary>
    /// Just a simplified version for now without taking into account stats of anything else.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IAttack GetAttack()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.AllComponents.GetFirstOrDefault<IEquipment>() is not IEquipment equipment)
            throw new InvalidOperationException("Parent is missing equipment component.");

        if (equipment.RightHand is RogueLikeEntity weapon &&
            weapon.AllComponents.GetFirstOrDefault<IMeleeAttack>() is IMeleeAttack meleeAttack)
        {
            var attackType = GlobalRandom.DefaultRNG.RandomElement(meleeAttack.Attacks.Keys.ToArray());
            return meleeAttack.Attacks[attackType];
        }
        else
            return Attack.None;
    }

    public AttackAction GetAttackAction(RogueLikeEntity target)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (target.AllComponents.GetFirstOrDefault<ICombatant>() is null)
            throw new InvalidOperationException("Target is not a combatant.");

        // Calculate attack time cost.
        int timeCost = GameSettings.TurnTime;

        // Create attack action.
        return new AttackAction(Parent, target, timeCost);
    }

    /// <summary>
    /// Just a simplified version for now without taking into account armor of anything else.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public IDefence GetDefence()
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.AllComponents.GetFirstOrDefault<IEquipment>() is not IEquipment equipment)
            throw new InvalidOperationException("Parent is missing equipment component.");

        if (equipment.LeftHand is RogueLikeEntity shield &&
            shield.AllComponents.GetFirstOrDefault<IDefence>() is IDefence defence)
        {
            return defence;
        }
        else
            return Defence.None;

    }

    /// <summary>
    /// Resolves the incoming <see cref="IAttack"/> and own <see cref="IDefence"/>
    /// applying the effect to <see cref="IHealth"/> if the attack goes through.
    /// </summary>
    /// <param name="attack">Attack being received.</param>
    public void Resolve(IAttack attack)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.AllComponents.GetFirstOrDefault<IHealth>() is not IHealth health)
            throw new InvalidOperationException("Parent is missing health component.");

        var defence = GetDefence();

        // Resolve physical attack.
        if (attack.PotentialPhysicalDamage != PotentialPhysicalDamage.None)
        {
            // Get attack damage.
            var physicalDamage = attack.PotentialPhysicalDamage.Resolve();

            // Get equivalent protection if available.
            var potentialProtection = defence.PotentialPhysicalProtections
                .Where(p => p.EffectType == attack.PotentialPhysicalDamage.EffectType)
                .FirstOrDefault();

            // Reduce damage by protection.
            if (potentialProtection != null)
            {
                var physicalProtection = potentialProtection.Resolve();
                int damageAmount = physicalDamage.Amount - physicalProtection.Amount;

                if (damageAmount <= 0)
                {
                    damageAmount = 0;
                    physicalDamage = PhysicalDamage.None;
                }
                else
                {
                    // Modify physical damage.
                    physicalDamage = physicalDamage with
                    {
                        Amount = damageAmount
                    };
                }
                    
            }

            // Send damage to health component.
            health.ReceiveDamage(physicalDamage);
        }

        // Resolve elemental damage.
        if (attack.PotentialElementalDamage != PotentialElementalDamage.None)
        {
            // Get attack damage.
            var elementalDamage = attack.PotentialElementalDamage.Resolve();

            // Get equivalent protection if available.
            var potentialProtection = defence.PotentialElementalProtections
                .Where(p => p.EffectType == attack.PotentialElementalDamage.EffectType)
                .FirstOrDefault();

            // Reduce damage by protection.
            if (potentialProtection != null)
            {
                var elementalProtection = potentialProtection.Resolve();
                int damageAmount = elementalDamage.Amount - elementalProtection.Amount;

                if (damageAmount <= 0)
                {
                    damageAmount = 0;
                    elementalDamage = ElementalDamage.None;
                }
                else
                {
                    elementalDamage = elementalDamage with
                    {
                        Amount = damageAmount
                    };
                }

            }

            // Send damage to health component.
            health.ReceiveDamage(elementalDamage);
        }
    }
}