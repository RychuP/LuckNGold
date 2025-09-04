namespace LuckNGold.World.Entities.Actors;

internal abstract class Enemy(string name, int glyph, Color color, int maxHP, int defense, int power, int invCapacity) 
    : Actor(name, glyph, color, maxHP, defense, power, invCapacity)
{ }