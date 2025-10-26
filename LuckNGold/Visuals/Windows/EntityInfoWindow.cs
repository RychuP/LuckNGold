using SadConsole.UI;

namespace LuckNGold.Visuals.Windows;

internal class EntityInfoWindow : Window
{
    const int DesiredWidth = 30;
    const int DesiredHeight = 50;

    public EntityInfoWindow() : base(DesiredWidth, DesiredHeight)
    {
        Hide();
    }

    public void ShowDescription(string entityName, string description)
    {
        Surface.Clear();
        Title = $"{entityName} Info";

        // Print text once to get sizes.
        Resize(DesiredWidth, DesiredHeight, true);
        Cursor.Position = Point.Zero;
        Cursor.Print(description);

        // Resize to sizes and print text again.
        var lines = GetDescriptionLines(out int maxLineLength);
        int width = maxLineLength + 0;
        int height = Cursor.Position.Y + 5;
        Resize(width, height, true);
        PrintLines(lines);

        // Draw border.
        DrawBorder();
    }

    void PrintLines(List<string> lines)
    {
        int y = 2;
        foreach (var line in lines)
            Surface.Print(1, y++, line);
    }

    List<string> GetDescriptionLines(out int maxLineLength)
    {
        maxLineLength = 0;
        List<string> lines = [];
        for (int y = 0; y < Cursor.Position.Y + 1; y++)
        {
            string line = Surface.GetString(0, y, Width).TrimEnd();
            if (line.Length > maxLineLength) 
                maxLineLength = line.Length;
            lines.Add(line);
        }
        return lines;
    }
}