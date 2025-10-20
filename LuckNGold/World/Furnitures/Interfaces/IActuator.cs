using LuckNGold.World.Furnitures.Enums;

namespace LuckNGold.World.Furnitures.Interfaces;

internal interface IActuator
{
    event EventHandler? StateChanged;
    ActuatorState State { get; }
    void Extend();
    void Retract();
}