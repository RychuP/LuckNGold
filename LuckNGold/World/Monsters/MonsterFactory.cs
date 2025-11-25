using GoRogue.Random;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;
using ShaiRandom.Generators;

namespace LuckNGold.World.Monsters;

/// <summary>
/// Factory that produces monster entities.
/// </summary>
static class MonsterFactory
{
    static readonly IEnhancedRandom rnd = GlobalRandom.DefaultRNG;

    public static RogueLikeEntity Player()
    {
        var identityComponent = new IdentityComponent("Henry", Race.Human);
        var age = (Age)rnd.NextInt(3);
        var hairCut = (HairCut)rnd.NextInt(4);
        var hairStyle = (HairStyle)rnd.NextInt(4);
        
        // Not all beard styles are available for each age.
        BeardStyle beardStyle = age switch
        {
            Age.Adult => (BeardStyle)rnd.NextInt(1, 3),
            Age.Old => (BeardStyle)rnd.NextInt(0, 2),
            _ => BeardStyle.None
        };

        // Create appearance.
        identityComponent.Appearance = identityComponent.Appearance with 
        { 
            Age = age, 
            HairStyle = hairStyle,
            HairCut = hairCut, 
            BeardStyle = beardStyle
        };

        var player = GetMonster("Player");
        player.AllComponents.Add(identityComponent);
        player.AllComponents.Add(new EquipmentComponent());
        player.AllComponents.Add(new OnionComponent());
        //player.AllComponents.Add(new InventoryComponent(20));
        player.AllComponents.Add(new WalletComponent());
        player.AllComponents.Add(new QuickAccessComponent());
        player.AllComponents.Add(new PlayerFOVController());
        player.AllComponents.Add(new HealthComponent(50));
        player.AllComponents.Add(new StatsComponent(2, 3, 1));
        player.AllComponents.Add(new LevelComponent());
        player.AllComponents.Add(new BumpableComponent());
        player.AllComponents.Add(new CombatantComponent());

        return player;
    }

    public static RogueLikeEntity Skeleton()
    {
        var skeleton = GetMonster("Skeleton");
        skeleton.AllComponents.Add(new IdentityComponent("Skelly", Race.Skeleton));
        skeleton.AllComponents.Add(new EquipmentComponent());
        skeleton.AllComponents.Add(new OnionComponent());
        skeleton.AllComponents.Add(new HealthComponent(20));
        skeleton.AllComponents.Add(new StatsComponent(2, 3, 1));
        skeleton.AllComponents.Add(new BumpableComponent());
        skeleton.AllComponents.Add(new CombatantComponent());

        return skeleton;
    }

    static RogueLikeEntity GetMonster(string name) =>
        new(4, false, layer: (int)GameMap.Layer.Monsters) { Name = name };
}