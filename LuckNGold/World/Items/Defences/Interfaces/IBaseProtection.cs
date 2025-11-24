namespace LuckNGold.World.Items.Defences.Interfaces;

interface IBaseProtection
{
    int MinProtection { get; }
    int MaxProtection { get; }
    int Resolve();
}