namespace LuckNGold.World.Items.Defences.Interfaces;

interface IPotentialProtection
{
    int MinProtection { get; }
    int MaxProtection { get; }
    int Resolve();
}