namespace LuckNGold.Generation.Decors;

/// <summary>
/// Steps leading to other levels.
/// </summary>
record Steps : Decor
{ 
    /// <summary>
    /// Whether the steps lead down or up.
    /// </summary>
    public bool LeadDown { get; init; }

    /// <summary>
    /// Whether the steps face left or right.
    /// </summary>
    public bool FaceRight { get; init; }

    public Steps(Point position, bool leadDown, bool faceRight) : base(position)
    {
        LeadDown = leadDown;
        FaceRight = faceRight;
    }
}