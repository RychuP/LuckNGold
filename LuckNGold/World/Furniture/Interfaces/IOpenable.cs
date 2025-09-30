namespace LuckNGold.World.Furniture.Interfaces;

internal interface IOpenable
{
    bool IsOpen { get; }
    bool Open();
    bool Close();
}