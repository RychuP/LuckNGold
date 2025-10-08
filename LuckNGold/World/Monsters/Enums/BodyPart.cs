using LuckNGold.World.Items.Interfaces;

namespace LuckNGold.World.Monsters.Enums;

/// <summary>
/// Parts of a monster entity that can be dressed with <see cref="IWearable"/>.
/// </summary>
/// <remarks>This game will primarily feature humanoid entities.</remarks>
enum BodyPart
{
    Head,
    Chest,
    Feet
}