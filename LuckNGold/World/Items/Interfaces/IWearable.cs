namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can be worn.
/// </summary>
internal interface IWearable : ICarryable
{
    bool PutOn();
    bool TakeOff();
}