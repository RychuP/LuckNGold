using LuckNGold.Generation.Monsters;
using LuckNGold.Generation.Monsters.Skeletons;
using LuckNGold.World.Items.Components.Interfaces;
using LuckNGold.World.Monsters;
using LuckNGold.World.Monsters.Components.Interfaces;
using SadRogue.Integration;

namespace LuckNGold.World.Map;

partial class GameMap
{
    public void PlaceMonster(Monster monster)
    {
        if (monster.Position == Point.None)
            throw new InvalidOperationException("Valid position is needed.");

        var monsterEntity = GetEntityAt<RogueLikeEntity>(monster.Position);
        if (monsterEntity is not null)
            throw new InvalidOperationException("Another entity already at location.");

        monsterEntity = CreateMonster(monster);
        CreateMonsterEquipment(monster, monsterEntity);
        AddEntity(monsterEntity, monster.Position);
    }

    void CreateMonsterEquipment(Monster monster, RogueLikeEntity monsterEntity)
    {
        var equipment = monsterEntity.AllComponents.GetFirst<IEquipment>();
        
        foreach (var equipSlot in monster.Equipment.Keys)
        {
            if (equipment.Equipment[equipSlot] is null)
            {
                var item = monster.Equipment[equipSlot];
                var itemEntity = CreateItem(item);

                var equippable = itemEntity.AllComponents.GetFirst<IEquippable>();
                if (equippable.Slot == equipSlot)
                    equipment.Equip(itemEntity, out RogueLikeEntity? _);
                else
                    throw new InvalidOperationException($"Invalid equipment for the {equipSlot} slot.");
            }
            else
                throw new InvalidOperationException($"Slot {equipSlot} is already taken.");
        }
    }

    public static RogueLikeEntity CreateMonster(Monster monster) =>
        monster is Skeleton ? MonsterFactory.Skeleton() :
        throw new NotImplementedException();
}