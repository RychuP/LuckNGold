using SadConsole.UI;
using System.Text;

namespace LuckNGold.Visuals.Windows;

/// <summary>
/// Window that display info about an entity selected with a pointer.
/// </summary>
internal class EntityInfoWindow : Window
{
    const int DesiredWidth = 30;
    const int DesiredHeight = 50;
    readonly string[] _articles = { "a", "an", "i", "the"};
    
    // Colors for the colored string parser.
    readonly string _foregroundColor;
    readonly string[] _highlights = ["LightGreen", "Yellow", "Tomato", "DeepSkyBlue", ];

    /// <summary>
    /// Initializes a new instance of <see cref="EntityInfoWindow"/> class.
    /// </summary>
    public EntityInfoWindow() : base(DesiredWidth, DesiredHeight)
    {
        Hide();
        PositionChanged += OnPositionChanged;
        _foregroundColor = $"{Surface.DefaultForeground.R},{Surface.DefaultForeground.G}," +
            $"{Surface.DefaultForeground.B},{Surface.DefaultForeground.A}";
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
            PrintLines(stateDescriptionLines, descriptionLines.Count + 3);
        DrawBorder();
    }

    void PrintLines(List<string> lines, int y)
    {
        foreach (var line in lines)
        {
            if (line.Contains('['))
            {
                var modifiedLine = $"[c:r f:{_foregroundColor}]{line}[c:u]";
                var coloredString = ColoredString.Parser.Parse(modifiedLine);
                Surface.Print(1, y++, coloredString);
            }
            else
            {
                Surface.Print(1, y++, line);
            }
        }
    }

    /// <summary>
    /// Breaks a long string down into an array of shorter ones and replaces #commands
    /// with SadConsole recolor commands.
    /// </summary>
    /// <remarks>Prepend a word with #0-9 command to recolor it word with a predefined
    /// highlight.</remarks>
    /// <param name="input">String to be broken down.</param>
    /// <param name="maxLineLength">Maxim number of characters per line.</param>
    /// <param name="maxAchievedLineLength">Actual achieved max line length.</param>
    /// <returns>List of text lines.</returns>
    public List<string> BreakString(string input, int maxLineLength, 
        out int maxAchievedLineLength)
    {
        // Length of the longest line created so far.
        maxAchievedLineLength = 0;

        if (string.IsNullOrEmpty(input))
            return [];

        // Current line length.
        int lineLength;

        string[] words = input.Split(' ');
        List<string> lines = [];
        StringBuilder sb = new();
        string lastWord = "";

        // List of recolor commands for the current line.
        Dictionary<int, string> coloredWords = [];

        for (int i = 0; i < words.Length; i++)
        {
            var coloredWord = string.Empty;
            var currentWord = words[i];

            // Check if the word has a recolor command.
            if (currentWord.Contains('#'))
            {
                coloredWord = currentWord;
                currentWord = currentWord[2..];
            }

            int currentLineLength = sb.Length + currentWord.Length;
            if (currentLineLength > maxLineLength)
            {
                // Move the short article words at the end of a line to the next line.
                if (_articles.Contains(lastWord.ToLower()))
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

            // If the word has a command, save it for later.
            if (!string.IsNullOrEmpty(coloredWord))
                coloredWords.Add(sb.Length, coloredWord);

            // Add current word to the line.
            sb.Append($"{currentWord} ");

            // Remember last word.
            lastWord = currentWord;
        }

        lineLength = AddLine();
        if (lineLength > maxAchievedLineLength)
            maxAchievedLineLength = lineLength;

        return lines;

        int AddLine()
        {
            int lineLength = sb.ToString().Trim().Length;

            // Replace words with #commands to words with SadConsole colored string commands.
            AddColorsToWords();
            coloredWords.Clear();

            // Add new line.
            lines.Add(sb.ToString().Trim().ToString());
            sb.Clear();

            // Return length of the new line.
            return lineLength;
        }

        void AddColorsToWords()
        {
            // Sadconsole recolor commands change the length of the string
            // and index offset is needed so that the command word can be found.
            int indexOffset = 0;

            foreach (var element in coloredWords)
            {
                int index = element.Key;
                var coloredWord = element.Value;

                char command = coloredWord[1];
                string trimmedWord = coloredWord[2..];

                if (trimmedWord[^1] == ',' || trimmedWord[^1] == '.')
                    trimmedWord = trimmedWord[..^1];

                coloredWord = command switch
                {
                    '0' => $"[c:r f:{_highlights[0]}]{trimmedWord}[c:u]",
                    '1' => $"[c:r f:{_highlights[1]}]{trimmedWord}[c:u]",
                    '2' => $"[c:r f:{_highlights[2]}]{trimmedWord}[c:u]",
                    '3' => $"[c:r f:{_highlights[3]}]{trimmedWord}[c:u]",
                    _ => $"{trimmedWord}",
                };

                sb.Replace(trimmedWord, coloredWord, index + indexOffset, trimmedWord.Length);
                indexOffset += coloredWord.Length - trimmedWord.Length;
            }
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

            Point rightSidePosition = Position + (Width - 1, 0);
            if (!Program.Bounds.Contains(rightSidePosition))
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