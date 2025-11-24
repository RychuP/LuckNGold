namespace LuckNGold.World.Monsters.Components.Interfaces;

/// <summary>
/// It has attributes that affect actions.
/// </summary>
interface IStats
{
    /// <summary>
    /// Attribute that affects type of weapons that can be wielded and their damage output.
    /// </summary>
    int Strength { get; }

    /// <summary>
    /// Attribute that affects ...
    /// </summary>
    int Dexterity { get; }

    /// <summary>
    /// Attribute that affects ..
    /// </summary>
    int Wisdom { get; }
}