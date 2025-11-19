using LuckNGold.Generation.Items;
using LuckNGold.Generation.Items.Weapons;
using LuckNGold.Generation.Items.Weapons.Swords;
using LuckNGold.World.Items;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

// Translates item data objects from generator to RogueLike entities.
partial class GameMap
{
    public void PlaceItem(Item item)
    {
        if (item.Position == Point.None)
            throw new InvalidOperationException("Valid position is needed.");

        var entity = GetEntityAt<RogueLikeEntity>(item.Position);
        if (entity is not null)
            throw new InvalidOperationException("Another entity already at location.");

        entity = CreateItem(item);
        AddEntity(entity, item.Position);
    }

    /// <summary>
    /// Converts data object into a <see cref="RogueLikeEntity"/> that can be placed on the map.
    /// </summary>
    /// <param name="item">Data object from generator.</param>
    /// <returns>An instance of <see cref="RogueLikeEntity"/> 
    /// created from given data object.</returns>
    /// <exception cref="ArgumentException"></exception>
    public static RogueLikeEntity CreateItem(Item item) =>
        item is Key key ? ItemFactory.Key(key.Material) :
        item is Coin ? ItemFactory.Coin() :
        item is Weapon weapon ? CreateWeapon(weapon) :
        throw new ArgumentException("Item not implemented.");

    static RogueLikeEntity CreateWeapon(Weapon weapon) =>
        weapon is Sword sword ? CreateSword(sword) :
        throw new ArgumentException("Weapon is not implemented.");

    static RogueLikeEntity CreateSword(Sword sword) =>
        sword is ArmingSword arming ? WeaponFactory.ArmingSword(arming.Material, arming.Attacks) :
        sword is GladiusSword gladius ? WeaponFactory.GladiusSword(gladius.Material, gladius.Attacks) :
        sword is ScimitarSword scimitar ? WeaponFactory.ScimitarSword(scimitar.Material, scimitar.Attacks) :
        throw new ArgumentException("Sword is not implemented.");
}