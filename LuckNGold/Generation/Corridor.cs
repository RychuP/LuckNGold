namespace LuckNGold.Generation;

internal class Corridor
{
    public Exit Start { get; }
    public Exit End { get; }

    public Corridor(Exit start, Exit end)
    {
        if (start == end)
            throw new ArgumentException("Corridor needs two distinct exits.");

        if (start.IsDouble != end.IsDouble)
            throw new ArgumentException("Corridor needs two exits with the same width.");

        Start = start;
        End = end;
    }

    public bool IsHorizontal() =>
        Start.Position.Y == End.Position.Y;

    public bool IsVertical() =>
        Start.Position.X == End.Position.X;

    public bool HasExit(Exit exit) =>
        Start == exit || End == exit;

    public List<Point> GetPositions()
    {
        List<Point> positions = [];
        positions.AddRange(Lines.GetOrthogonalLine(Start.Position, End.Position));
        if (Start.IsDouble)
        {
            if (Start.Direction.IsHorizontal() || End.Direction.IsHorizontal())
                throw new InvalidOperationException("Horizontal corridor should not " +
                    "be marked as double.");

            Point startDouble = Start.Position + Direction.Right;
            Point endDouble = End.Position + Direction.Right;
            positions.AddRange(Lines.GetOrthogonalLine(startDouble, endDouble));
        }
        return positions;
    }
}