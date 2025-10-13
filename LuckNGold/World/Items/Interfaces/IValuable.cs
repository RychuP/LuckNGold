﻿namespace LuckNGold.World.Items.Interfaces;

/// <summary>
/// It can by purchased and sold by traders.
/// </summary>
internal interface IValuable
{
    /// <summary>
    /// Monetary value in coins.
    /// </summary>
    int Value { get; }
}