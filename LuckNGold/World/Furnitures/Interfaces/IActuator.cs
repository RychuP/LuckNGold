using LuckNGold.World.Furnitures.Enums;

namespace LuckNGold.World.Furnitures.Interfaces;

/// <summary>
/// It can extend and retract like an actuator or a piston.
/// </summary>
internal interface IActuator
{
    event EventHandler? StateChanged;
    ActuatorState State { get; }
    void Extend();
    void Retract();
}