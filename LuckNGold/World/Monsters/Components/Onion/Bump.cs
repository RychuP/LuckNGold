using SadConsole.EasingFunctions;
using SadConsole.Instructions;

namespace LuckNGold.World.Monsters.Components;

partial class OnionComponent
{
    public bool IsBumping { get; private set; }

    public void Bump(int pixelCount, Direction direction)
    {
        IsBumping = true;

        // Change cell positioning to pixel positioning.
        int pixelX = CurrentFrame.Position.X * CurrentFrame.FontSize.X;
        int pixelY = CurrentFrame.Position.Y * CurrentFrame.FontSize.Y;
        CurrentFrame.UsePixelPositioning = true;
        CurrentFrame.Position = (pixelX, pixelY);

        // Make sure the frame is on top of other entities.
        var parent = CurrentFrame.Parent!;
        parent.Children.Add(CurrentFrame);

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
        var position = CurrentFrame.Position;

        var animatedValue = new AnimatedValue(duration, 0, pixelCount, new Quad())
        {
            RemoveOnFinished = true
        };

        animatedValue.ValueChanged += (o, d) =>
        {
            int x = position.X + direction.DeltaX * (int)d;
            int y = position.Y + direction.DeltaY * (int)d;
            CurrentFrame.Position = (x, y);
        };

        Parent.AllComponents.Add(animatedValue);
        return animatedValue;
    }
}