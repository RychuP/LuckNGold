namespace LuckNGold.Generation.Furnitures;

record Gate : Entryway
{
    public bool IsOperateRemotely { get; init; }

    public Gate(Point position, Direction direction, bool isOperatedRemotely = true) 
        : base(position, direction)
    {
        IsOperateRemotely = isOperatedRemotely;
    }
}