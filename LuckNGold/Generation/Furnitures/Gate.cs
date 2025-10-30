using LuckNGold.Generation.Map;

namespace LuckNGold.Generation.Furnitures;

record Gate : Entryway
{
    public bool IsOperateRemotely { get; init; }

    public Gate(Exit exit, bool isOperatedRemotely = true) : base(exit)
    {
        IsOperateRemotely = isOperatedRemotely;
    }
}