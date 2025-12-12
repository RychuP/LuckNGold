using SadConsole.EasingFunctions;
using SadConsole.Instructions;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    public event EventHandler? IsBumpingChanged;

    public Point BumpPosition { get; set; } = Point.None;

    bool _isBumping = false;
    public bool IsBumping
    {
        get => _isBumping;
        private set
        {
            if (_isBumping == value) return;
            _isBumping = value;
            OnIsBumpingChanged();
        }
    }

    public void Bump(int pixelCount, Direction direction)
    {
        if (CurrentFrame.Parent is null)
            throw new InvalidOperationException("Current frame is not added to monster layer.");

        IsBumping = true;
        var monsterLayer = CurrentFrame.Parent;

        // Change cell positioning to pixel positioning.
        int pixelX = CurrentFrame.Position.X * CurrentFrame.FontSize.X;
        int pixelY = CurrentFrame.Position.Y * CurrentFrame.FontSize.Y;
        CurrentFrame.UsePixelPositioning = true;
        CurrentFrame.Position = (pixelX, pixelY);

        // Make sure the current frame is properly layered depending on the direction of the bump.
        if (direction.DeltaY <= 0)
            monsterLayer.Children.MoveToTop(CurrentFrame);
        else
            monsterLayer.Children.MoveToBottom(CurrentFrame);

        // Create an animated value component that manages the bump.
        var animatedValue = BumpBase(pixelCount, direction);

        // Workaround to the problem with Finished event.
        animatedValue.ValueChanged += (o, d) =>
        {
            if (animatedValue.Value == pixelCount)
            {
                var oppositeDirection = Direction.GetDirection(-direction.DeltaX, -direction.DeltaY);
                ReturnFromBump(pixelCount, oppositeDirection);
            }
        };
    }

    void ReturnFromBump(int pixelCount, Direction direction)
    {
        var animatedValue = BumpBase(pixelCount, direction);

        // Workaround to the problem with Finished event.
        animatedValue.ValueChanged += (o, d) =>
        {
            if (animatedValue.Value == pixelCount)
            {
                // Return to cell positioning.
                int x = CurrentFrame.Position.X / CurrentFrame.FontSize.X;
                int y = CurrentFrame.Position.Y / CurrentFrame.FontSize.Y;
                CurrentFrame.UsePixelPositioning = false;
                CurrentFrame.Position = (x, y);

                IsBumping = false;
            }
        };
    }

    AnimatedValue BumpBase(int pixelCount, Direction direction)
    {
        if (Parent == null)
            throw new InvalidOperationException("Component needs to be attached to an entity.");

        var duration = TimeSpan.FromMilliseconds(200);
        BumpPosition = CurrentFrame.Position;

        var animatedValue = new AnimatedValue(duration, 0, pixelCount, new Quad())
        {
            RemoveOnFinished = true
        };

        animatedValue.ValueChanged += (o, d) =>
        {
            int x = BumpPosition.X + direction.DeltaX * (int)d;
            int y = BumpPosition.Y + direction.DeltaY * (int)d;
            CurrentFrame.Position = (x, y);
        };

        Parent.AllComponents.Add(animatedValue);
        return animatedValue;
    }

    void OnIsBumpingChanged()
    {
        IsBumpingChanged?.Invoke(this, EventArgs.Empty);
    }
}