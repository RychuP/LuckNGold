namespace LuckNGold.World.Furnitures.Interfaces;

/// <summary>
/// It can receive signals from remote transmitters.
/// </summary>
interface ISignalReceiver
{
    event EventHandler? SignalReceived;
    event EventHandler? SignalConsumed;
    bool HasUnconsumedSignal { get; }
    void ReceiveSignal();
    void ConsumeSignal();
}