namespace LuckNGold.Generation.Monsters.Skeletons;

abstract record Skeleton : Monster
{
    public Skeleton(Point position) : base(position) { }
}