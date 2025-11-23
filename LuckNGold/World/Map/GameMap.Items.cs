using LuckNGold.Generation.Items;
using LuckNGold.Generation.Items.Bodywears;
using LuckNGold.Generation.Items.Bodywears.Armours;
using LuckNGold.Generation.Items.Bodywears.Clothings;
using LuckNGold.Generation.Items.Collectables;
using LuckNGold.Generation.Items.Footwears;
using LuckNGold.Generation.Items.Footwears.Boots;
using LuckNGold.Generation.Items.Footwears.Shoes;
using LuckNGold.Generation.Items.Helmets;
using LuckNGold.Generation.Items.Shields;
using LuckNGold.Generation.Items.Tools;
using LuckNGold.Generation.Items.Weapons;
using LuckNGold.Generation.Items.Weapons.Swords;
using LuckNGold.World.Items;
using LuckNGold.World.Items.Materials.Interfaces;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

// Translates item data objects from generator to RogueLike entities.
partial class GameMap
{
    public static RogueLikeEntity CreateItem(Item item) =>
        item is Tool tool ? CreateTool(tool) :
        item is Weapon weapon ? CreateWeapon(weapon) :
        item is Shield shield ? CreateShield(shield) :
        item is Bodywear bodywear ? CreateBodywear(bodywear) :
        item is Footwear footwear ? CreateFootwear(footwear) :
        item is Helmet helmet ? CreateHelmet(helmet) :
        item is Collectable collectable ? CreateCollectable(collectable) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateWeapon(Weapon weapon) =>
        weapon is Sword sword ? CreateSword(sword) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateSword(Sword sword) =>
        sword is ArmingSword arming ? WeaponFactory.ArmingSword(arming.Material, arming.Attacks) :
        sword is GladiusSword gladius ? WeaponFactory.GladiusSword(gladius.Material, gladius.Attacks) :
        sword is ScimitarSword scimitar ? WeaponFactory.ScimitarSword(scimitar.Material, scimitar.Attacks) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateBodywear(Bodywear bodywear) =>
        bodywear is Clothing clothing ? CreateClothing(clothing) :
        bodywear is Armour armour ? CreateArmour(armour) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateClothing(Clothing clothing) =>
        clothing is LinenClothing ? ClothingFactory.LinenClothing() :
        throw new NotImplementedException();

    static RogueLikeEntity CreateArmour(Armour armour) =>
        throw new NotImplementedException();

    static RogueLikeEntity CreateFootwear(Footwear footwear) =>
        footwear is Shoe shoes ? CreateShoes(shoes) :
        footwear is Boot boots ? CreateBoots(boots) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateShoes(Shoe shoes) =>
        shoes is PeasantShoes ? FootwearFactory.PeasantShoes(shoes.Material) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateBoots(Boot boots) =>
        throw new NotImplementedException();

    static RogueLikeEntity CreateHelmet(Helmet helmet) =>
        helmet is BanditHelmet ? ArmourFactory.BanditHelmet(helmet.Material) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateShield(Shield shield) =>
        shield is BanditShield ? ArmourFactory.BanditShield(shield.Material) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateTool(Tool tool) =>
        tool is Key key ? ToolFactory.Key((IGemstone)key.Material) :
        throw new NotImplementedException();

    static RogueLikeEntity CreateCollectable(Collectable collectable) =>
        collectable is Coin ? CollectableFactory.Coin() :
        throw new NotImplementedException();

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
}