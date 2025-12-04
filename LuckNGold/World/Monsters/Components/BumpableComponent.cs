using LuckNGold.World.Monsters.Components.Interfaces;
using SadConsole.EasingFunctions;
using SadRogue.Integration;
using SadRogue.Integration.Components;

namespace LuckNGold.World.Monsters.Components;

/// <summary>
/// Component for entities that can respond to bumps.
/// </summary>
internal class BumpableComponent() :
    RogueLikeComponentBase<RogueLikeEntity>(false, false, false, false), IBumpable
{
    public void OnBumped(RogueLikeEntity source)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        FaceOponent(Parent, source);

        if (Parent.AllComponents.GetFirstOrDefault<ICombatant>() is ICombatant parentCombatant &&
            source.AllComponents.GetFirstOrDefault<ICombatant>() is ICombatant sourceCombatant)
        {
            var attack = sourceCombatant.GetAttack();
            parentCombatant.Resolve(attack);
        }
    }

    static void FaceOponent(RogueLikeEntity one, RogueLikeEntity two)
    {
        if (one.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent)
        {
            var deltaChange = two.Position - one.Position;
            var direction = Direction.GetDirection(deltaChange);
            onionComponent.FaceDirection(direction);
        }
    }

    public void OnBumping(RogueLikeEntity target)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        FaceOponent(Parent, target);
        
        if (Parent.AllComponents.GetFirstOrDefault<IOnion>() is IOnion onionComponent)
        {
            var direction = Direction.GetDirection(Parent.Position, target.Position);
            int pixelCount = onionComponent.CurrentFrame.FontSize.X / 4;
            onionComponent.Bump(pixelCount, direction);
        }
    }
}