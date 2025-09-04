namespace LuckNGold.World.Entities.Items;

internal abstract class Item(string name, int glyph, Color color) 
    : Entity(name, glyph, color, EntityLayer.Items, true, true)
{ }