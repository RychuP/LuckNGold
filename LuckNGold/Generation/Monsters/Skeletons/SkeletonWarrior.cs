using LuckNGold.Generation.Items.Footwears.Shoes;
using LuckNGold.Generation.Items.Helmets;
using LuckNGold.Generation.Items.Weapons.Swords;
using LuckNGold.World.Items.Materials;
using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.Generation.Monsters.Skeletons;

record SkeletonWarrior : Skeleton
{
    public SkeletonWarrior(Point position) : base(position)
    {
        var sword = new GladiusSword(Point.None, Metal.MoonSteel);
        Equipment[EquipSlot.RightHand] = sword;

        var shoes = new PeasantShoes(Point.None, Leather.BovineHide);
        Equipment[EquipSlot.Feet] = shoes;
    }
}