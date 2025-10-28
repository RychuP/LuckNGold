using SadConsole.UI;
using System.Text;

namespace LuckNGold.Visuals.Windows;

internal class EntityInfoWindow : Window
{
    const int DesiredWidth = 30;
    const int DesiredHeight = 50;

    public EntityInfoWindow() : base(DesiredWidth, DesiredHeight)
    {
        Hide();
        PositionChanged += OnPositionChanged;
    }

    public void ShowDescription(string entityName, string description, string stateDescription)
    {
        Surface.Clear();
        Title = $"{entityName} Info";

        var descriptionLines = BreakString(description, DesiredWidth, 
            out int descMaxLineLength);
        var stateDescriptionLines = BreakString(stateDescription, DesiredWidth,
            out int stateDescMaxLineLength);
        int maxLineLength = descMaxLineLength > stateDescMaxLineLength ?
            descMaxLineLength : stateDescMaxLineLength;
        int width = maxLineLength + 2;
        int height = descriptionLines.Count + 3;
        if (!string.IsNullOrEmpty(stateDescription))
            height += stateDescriptionLines.Count + 1;

        Resize(width, height, true);
        PrintLines(descriptionLines, 2);
        if (!string.IsNullOrEmpty(stateDescription))
            PrintLines(stateDescriptionLines, stateDescriptionLines.Count + 2);
        DrawBorder();
    }

    void PrintLines(List<string> lines, int y)
    {
        foreach (var line in lines)
            Surface.Print(1, y++, line);
    }

    /// <summary>
    /// Breaks a long string down into an array of shorter ones.
    /// </summary>
    /// <param name="input">String to be broken down.</param>
    /// <param name="maxLineLength">Maxim number of characters per line.</param>
    /// <returns>List of text lines.</returns>
    public static List<string> BreakString(string input, int maxLineLength, 
        out int maxAchievedLineLength)
    {
        int lineLength;
        maxAchievedLineLength = 0;

        if (string.IsNullOrEmpty(input))
            return [];

        string[] words = input.Split(' ');
        List<string> lines = [];
        StringBuilder sb = new();

        string[] articles = { "a", "an", "and", "is", "are", "were", "was", "i", "the", "it"};
        string lastWord = "";

        for (int i = 0; i < words.Length; i++)
        {
            if (sb.Length + words[i].Length > maxLineLength)
            {
                // Move the short article words at the end of a line to the next line.
                if (articles.Contains(lastWord.ToLower()))
                {
                    sb.Remove(sb.Length - lastWord.Length - 1, lastWord.Length);
                    lineLength = AddLine();
                    if (lineLength > maxAchievedLineLength)
                        maxAchievedLineLength = lineLength;
                    sb.Append(lastWord + " ");

                }
                else
                {
                    lineLength = AddLine();
                    if (lineLength > maxAchievedLineLength)
                        maxAchievedLineLength = lineLength;
                }
            }

            // add word to the line
            sb.Append(words[i] + " ");
            lastWord = words[i];
        }

        lineLength = AddLine();
        if (lineLength > maxAchievedLineLength)
            maxAchievedLineLength = lineLength;
        return lines;

        int AddLine()
        {
            var line = sb.ToString().TrimEnd();
            lines.Add(line);
            sb.Clear();
            return line.Length;
        }
    }

    /// <summary>
    /// Moves position left or right to fit the window within the screen bounds.
    /// </summary>
    void OnPositionChanged(object? o, EventArgs e)
    {
        if (Width >= Program.Width) return;

        Point horizontalDelta;
        do
        {
            horizontalDelta = Point.Zero;

            if (!Program.Bounds.Contains(Position.WithX(Surface.Area.MaxExtentX)))
            {
                horizontalDelta = Point.Zero + Direction.Left;
            }
            else if (!Program.Bounds.Contains(Position))
            {
                horizontalDelta = Point.Zero + Direction.Right;
            }

            Position += horizontalDelta;
        }
        while (horizontalDelta != Point.Zero);
    }
}