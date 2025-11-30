using SadConsole.EasingFunctions;
using SadConsole.Input;
using SadConsole.Instructions;

namespace LuckNGold.Tests;

internal class BumpTest : SimpleSurface
{
    public BumpTest()
    {
        Surface.Print(1, 1, "Press VI motions for bump tests.");
        var box = new Box();
        box.SetPosition(5, 5);
        Children.Add(box);
    }
}

class Box : ScreenSurface
{
    const int bumpPixelCount = 20;

    public Box() : base(1, 1)
    {
        FontSize = (40, 40);
        IsFocused = true;
        Surface.DefaultBackground = Color.CornflowerBlue;
        Surface.Clear();
        UsePixelPositioning = true;
    }

    public void SetPosition(int x, int y)
    {
        x *= FontSize.X;
        y *= FontSize.Y;
        Position = (x, y);
    }

    void BumpForward(int pixelCount, Direction direction)
    {
        var animatedValue = Bump(pixelCount, direction);

        // Work around to the problem with Finished event.
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
        var animatedValue = Bump(pixelCount, direction);

        // Work around to the problem with Finished event.
        animatedValue.ValueChanged += (o, d) =>
        {
            if (animatedValue.Value == pixelCount)
            {
                // Nothing here yet.
            }
        };
    }

    AnimatedValue Bump(int pixelCount, Direction direction)
    {
        var duration = TimeSpan.FromMilliseconds(200);
        var position = Position;

        var animatedValue = new AnimatedValue(duration, 0, pixelCount, new Quad())
        {
            RemoveOnFinished = true
        };

        animatedValue.ValueChanged += (o, d) =>
        {
            int x = position.X + direction.DeltaX * (int)d;
            int y = position.Y + direction.DeltaY * (int)d;
            Position = (x, y);
        };

        SadComponents.Add(animatedValue);
        return animatedValue;
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.HasKeysPressed)
        {
            Direction direction = Direction.None;

            if (keyboard.IsKeyPressed(Keys.K))
                direction = Direction.Up;
            else if (keyboard.IsKeyPressed(Keys.J))
                direction = Direction.Down;

            if (keyboard.IsKeyPressed(Keys.H))
                direction = Direction.Left;
            else if (keyboard.IsKeyPressed(Keys.L))
                direction = Direction.Right;

            if (keyboard.IsKeyPressed(Keys.Y))
                direction = Direction.UpLeft;
            else if (keyboard.IsKeyPressed(Keys.N))
                direction = Direction.DownRight;

            if (keyboard.IsKeyPressed(Keys.U))
                direction = Direction.UpRight;
            else if (keyboard.IsKeyPressed(Keys.B))
                direction = Direction.DownLeft;

            if (direction != Direction.None)
            {
                BumpForward(bumpPixelCount, direction);
                return true;
            }
        }

        return base.ProcessKeyboard(keyboard);
    }
}