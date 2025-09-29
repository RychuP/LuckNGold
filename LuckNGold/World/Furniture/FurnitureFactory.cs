using LuckNGold.World.Furniture.Components;
using LuckNGold.World.Items.Enums;
using LuckNGold.World.Map;
using SadRogue.Integration;

namespace LuckNGold.World.Furniture;


/// <summary>
/// Factory of entities that can be interacted with like doors, chests, levers, etc
/// </summary>
internal class FurnitureFactory
{
    public static RogueLikeEntity Door(bool locked = false, KeyColor keyColor = KeyColor.None)
    {
        var glyphDef = Program.Font.GetGlyphDefinition("Door");
        var appearance = glyphDef.CreateColoredGlyph();

        var door = new RogueLikeEntity(appearance, !locked, !locked, (int)GameMap.Layer.Furniture);
        if (locked)
        {
            if (keyColor == KeyColor.None)
                throw new ArgumentException("Locked door requires a key color.");

            var lockComp = new LockComponent(keyColor);
            door.AllComponents.Add(lockComp);
        }
        return door;
    }
}