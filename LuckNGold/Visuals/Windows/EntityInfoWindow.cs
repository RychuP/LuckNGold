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
    }

    public void ShowDescription(string entityName, string description)
    {
        Surface.Clear();
        Title = $"{entityName} Info";

        var lines = BreakString(description, DesiredWidth, out int maxLineLength);
        int width = maxLineLength + 2;
        int height = lines.Count + 4;
        Resize(width, height, true);
        PrintLines(lines);
        DrawBorder();
    }

    void PrintLines(List<string> lines)
    {
        int y = 2;
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

        string[] articles = { "a", "an", "and", "is", "are", "were", "was", "i", "the" };
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
}