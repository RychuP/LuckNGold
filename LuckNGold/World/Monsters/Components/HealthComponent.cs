using LuckNGold.World.Items.Damage.Interfaces;
using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for monster entities that have hit points and can receive buffs and damage.
/// </summary>
/// <param name="health">Initial hit points.</param>
internal class HealthComponent(int health) :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IHealth
{
    public event EventHandler<ValueChangedEventArgs<int>>? HPChanged;
    public event EventHandler<IPhysicalDamage>? PhysicalDamageReceived;
    public event EventHandler<IElementalDamage>? ElementalDamageReceived;

    int _hp = health;
    public int HP
    {
        get => _hp;
        set
        {
            if (_hp == value) return;
            int prevHP = _hp;
            _hp = value;
            OnHPChanged(prevHP, value);
        }
    }

    void OnHPChanged(int prevHP, int newHP)
    {
        var args = new ValueChangedEventArgs<int>(prevHP, newHP);
        HPChanged?.Invoke(this, args);
    }

    public void ReceiveDamage(IPhysicalDamage physicalDamage)
    {
        HP -= physicalDamage.Amount;
        OnReceivedPhysicalDamage(physicalDamage);
    }

    public void ReceiveDamage(IElementalDamage elementalDamage)
    {
        HP -= elementalDamage.Amount;
        OnReceivedElementalDamage(elementalDamage);
    }

    void OnReceivedPhysicalDamage(IPhysicalDamage physicalDamage)
    {
        PhysicalDamageReceived?.Invoke(this, physicalDamage);
    }

    void OnReceivedElementalDamage(IElementalDamage elementalDamage)
    {
        ElementalDamageReceived?.Invoke(this, elementalDamage);
    }
}