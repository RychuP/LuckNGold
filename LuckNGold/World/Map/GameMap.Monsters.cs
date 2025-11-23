using LuckNGold.Generation.Decors;
using LuckNGold.Generation.Monsters;
using LuckNGold.Generation.Monsters.Skeletons;
using LuckNGold.World.Monsters;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

partial class GameMap
{
    public void PlaceMonster(Monster monster)
    {
        if (monster.Position == Point.None)
            throw new InvalidOperationException("Valid position is needed.");

        var entity = GetEntityAt<RogueLikeEntity>(monster.Position);
        if (entity is not null)
            throw new InvalidOperationException("Another entity already at location.");

        entity = CreateMonster(monster);
        AddEntity(entity, monster.Position);
    }

    public static RogueLikeEntity CreateMonster(Monster monster) =>
        monster is Skeleton ? MonsterFactory.Skeleton() :
        throw new NotImplementedException();
}