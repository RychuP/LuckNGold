using LuckNGold.World.Monsters.Enums;

namespace LuckNGold.World.Monsters.Interfaces;

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
    /// Layer 1 for weapons far.
    /// </summary>
    IOnionLayer WeaponFar { get; }

    /// <summary>
    /// Layer 2 for shields far.
    /// </summary>
    IOnionLayer ShieldFar { get; }

    /// <summary>
    /// Layer 3 for base appearance.
    /// </summary>
    IOnionLayer Base { get; }

    /// <summary>
    /// Layer 4 for clothes / armour.
    /// </summary>
    IOnionLayer ClothesArmour { get; }

    /// <summary>
    /// Layer 5 for beard.
    /// </summary>
    IOnionLayer Beard { get; }

    /// <summary>
    /// Layer 6 for hair / helmet.
    /// </summary>
    IOnionLayer HairHelmet { get; }

    /// <summary>
    /// Layer 7 for weapon near.
    /// </summary>
    IOnionLayer WeaponNear { get; }

    /// <summary>
    /// Layer 8 for weapon hand / empty right hand.
    /// </summary>
    IOnionLayer WeaponRightHand { get; }

    /// <summary>
    /// Layer 9 for shield near / empty left hand.
    /// </summary>
    IOnionLayer ShieldLeftHand { get; }
}