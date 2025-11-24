using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Primitives.Interfaces;

/// <summary>
/// It contains onion layers stacked on top of each other that form monster appearance.
/// </summary>
internal interface ILayerStack : IOnionLayer
{
    /// <summary>
    /// Sets font size of each layer by multiplying font size one and given multiplier.
    /// </summary>
    /// <param name="multiplier">Font size multiplier.</param>
    void SetFontSize(int multiplier);

    /// <summary>
    /// Gets layer with the given name.
    /// </summary>
    /// <param name="layerName">Name of the layer to be returned.</param>
    IOnionLayer GetLayer(OnionLayerName layerName);

    /// <summary>
    /// Layer 0 for weapons far.
    /// </summary>
    IOnionLayer WeaponFar { get; }

    /// <summary>
    /// Layer 1 for shields far.
    /// </summary>
    IOnionLayer ShieldFar { get; }

    /// <summary>
    /// Layer 2 for base appearance.
    /// </summary>
    IOnionLayer Base { get; }

    /// <summary>
    /// Layer 3 for clothes, robes, armour.
    /// </summary>
    IOnionLayer Bodywear { get; }

    /// <summary>
    /// Layer 4 for shoes, boots.
    /// </summary>
    IOnionLayer Footwear { get; }

    /// <summary>
    /// Layer 5 for beard.
    /// </summary>
    IOnionLayer Beard { get; }

    /// <summary>
    /// Layer 6 for hair, helmets.
    /// </summary>
    IOnionLayer Headwear { get; }

    /// <summary>
    /// Layer 7 for weapon near.
    /// </summary>
    IOnionLayer WeaponNear { get; }

    /// <summary>
    /// Layer 8 for empty right hand, weapon hand.
    /// </summary>
    IOnionLayer RightHand { get; }

    /// <summary>
    /// Layer 9 for empty left hand, shield near.
    /// </summary>
    IOnionLayer LeftHand { get; }
}