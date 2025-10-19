using LuckNGold.World.Furnitures.Interfaces;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Furnitures.Components;

class SignalReceiverComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), ISignalReceiver
{
    public event EventHandler? SignalReceived;
    public event EventHandler? SignalConsumed;

    bool _hasUnconsumedSignal = false;
    public bool HasUnconsumedSignal
    {
        get => _hasUnconsumedSignal;
        private set
        {
            if (_hasUnconsumedSignal == value) return;
            _hasUnconsumedSignal = value;

            if (_hasUnconsumedSignal == true)
                OnSignalReceived();
            else
                OnSignalConsumed();
        }
    }

    public void ConsumeSignal()
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap is null)
            throw new InvalidOperationException("Parent entity has to be on the map.");

        HasUnconsumedSignal = false;
    }

    public void ReceiveSignal()
    {
        if (Parent is null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        if (Parent.CurrentMap is null)
            throw new InvalidOperationException("Parent entity has to be on the map.");

        HasUnconsumedSignal = true;
    }

    void OnSignalReceived()
    {
        SignalReceived?.Invoke(this, EventArgs.Empty);
    }

    void OnSignalConsumed()
    {
        SignalConsumed?.Invoke(this, EventArgs.Empty);
    }
}
