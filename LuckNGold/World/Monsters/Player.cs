using LuckNGold.World.Monsters.Components;
using SadRogue.Integration;
using SadRogue.Integration.Keybindings;

namespace LuckNGold.World.Monsters;

internal class Player : RogueLikeEntity
{
    public Player() : base(2, false, layer: (int)GameMap.Layer.Monsters)
    {
        // Add component that handles controls.
        var motionControl = new CustomKeybindingsComponent();
        motionControl.SetMotions(KeybindingsComponent.ArrowMotions);
        motionControl.SetMotions(KeybindingsComponent.NumPadAllMotions);
        AllComponents.Add(motionControl);

        // Add component for updating map's player FOV as they move
        AllComponents.Add(new PlayerFOVController());
    }
}