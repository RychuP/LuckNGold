using GoRogue.Random;
using LuckNGold.World.Map;
using LuckNGold.World.Monsters.Components;
using LuckNGold.World.Monsters.Enums;
using LuckNGold.World.Monsters.Primitives;
using SadRogue.Integration;

namespace LuckNGold.World.Monsters;

static class MonsterFactory
{
    public static RogueLikeEntity Player()
    {
        var player = new RogueLikeEntity(2, false, layer: (int)GameMap.Layer.Monsters)
        {
            Name = "Player",
        };

        var rnd = GlobalRandom.DefaultRNG;
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

        // Add component that holds identity information.
        player.AllComponents.Add(identityComponent);

        // Add component that represents the equipment worn and carried.
        player.AllComponents.Add(new EquipmentComponent());

        // Add component that composes appearance of several layers.
        player.AllComponents.Add(new OnionComponent());

        // Add inventory component.
        player.AllComponents.Add(new InventoryComponent(20));

        // Add wallet to hold coins.
        player.AllComponents.Add(new WalletComponent());

        // Add quick access component displayed at the bottom of the screen.
        player.AllComponents.Add(new QuickAccessComponent());

        // Add component for updating map's player FOV as they move.
        player.AllComponents.Add(new PlayerFOVController());

        return player;
    }
}