using SadConsole.Components;
using SadConsole.Entities;
using SadConsole.Instructions;
using SadRogue.Integration;

namespace LuckNGold.Visuals.Windows;

/// <summary>
/// Surface that show a character entity marching in all cardinal directions.
/// </summary>
internal class CharacterPreviewSurface : ScreenSurface
{
    const int BoxSize = 7;
    const int StartIndex = BoxSize - 1;
    readonly RogueLikeEntity _characterEntity;

    // Positions that the character entity patrols.
    readonly Point[] _perimeterPositions;
    
    // Current index in the perimeter positions.
    int _positionIndex = StartIndex;

    /// <summary>
    /// Initializes an instance of <see cref="CharacterPreviewSurface"/> class.
    /// </summary>
    /// <param name="characterEntity">Character entity to be shown in preview.</param>
    public CharacterPreviewSurface(RogueLikeEntity characterEntity) : base(BoxSize, BoxSize)
    {
        _characterEntity = characterEntity;

        Font = Program.Font;
        FontSize *= 5;

        // Reduce view to 1 cell.
        ViewHeight = 1;
        ViewWidth = 1;

        // Get patrol positions.
        _perimeterPositions = [..Surface.Area.PerimeterPositions()];

        // Instruction that changes the position of the character entity.
        var instruction = new InstructionSet()
            .Wait(TimeSpan.FromMilliseconds(450))
            .Code(PatrolEntity);
        instruction.RepeatCount = -1;
        SadComponents.Add(instruction);

        // Component that centers view on the player.
        var followTargetComp = new SurfaceComponentFollowTarget();
        followTargetComp.Target = _characterEntity;
        SadComponents.Add(followTargetComp);

        // Entity manager that displays the character entity.
        var entityManager = new EntityManager { _characterEntity };
        SadComponents.Add(entityManager);

        PreparePatrol();
    }

    void PatrolEntity()
    {
        _characterEntity.Position = _perimeterPositions[_positionIndex++];
        if (_positionIndex == _perimeterPositions.Length)
            _positionIndex = 0;
    }

    // Prepares entity for a march down, so the first frame appears correct.
    void PreparePatrol()
    {
        _characterEntity.Position = _perimeterPositions[StartIndex] + Direction.Up;
    }

    protected override void OnVisibleChanged()
    {
        base.OnVisibleChanged();

        IsEnabled = IsVisible;
        if (IsVisible == true)
            _positionIndex = StartIndex;

        PreparePatrol();
    }
}