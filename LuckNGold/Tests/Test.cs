using LuckNGold.Visuals;

namespace LuckNGold.Tests;

internal class Test : SimpleSurface
{
    //ColoredGlyph _rectAppearance = new(Color.CornflowerBlue, Color.Black, 178);
    //ShapeParameters _shapeParams = ShapeParameters.CreateBorder(appearance);

    public Test()
    {
        string text = "#1Test,";
        Surface.Print(1, 1, text);

        if (text.Contains('#'))
        {
            char command = text[1];
            string trimmedText = text.Substring(2);
            Surface.Print(1, 2, trimmedText);

            string punctuation = string.Empty;
            if (trimmedText[^1] == ',' || trimmedText[^1] == '.')
            {
                punctuation = trimmedText[^1].ToString();
                trimmedText = trimmedText[..^1];
            }

            switch (command)
            {
                case '1':
                    text = $"[c:r f:LightGreen]{trimmedText}[c:u]{punctuation}";
                    break;
            }

            var coloredText = ColoredString.Parser.Parse(text);
            Surface.Print(1, 2, coloredText);
        }

        
    }
}