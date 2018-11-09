using Nokia3310.Applications.Common;
using RLNET;

namespace Nokia3310.Applications.Extensions
{
    public static class ConsoleExtensions
    {
        public static void DrawBox(this RLRootConsole console, int x, int y, int width, int height, RLColor borderColor, RLColor fillColor)
        {
            for (int i = x; i < x + width + 1; i++)
            {
                for (int j = y; j < y + height + 1; j++)
                {
                    console.SetChar(i, j, ' ');
                    console.SetColor(i, j, fillColor);
                    console.SetBackColor(i, j, fillColor);
                }
            }

            console.SetColor(x, y, borderColor);
            console.SetChar(x, y, Glyph.BoxTopLeft);
            console.SetColor(x, y + height, borderColor);
            console.SetChar(x, y + height, Glyph.BoxBottomLeft);
            console.SetColor(x + width, y, borderColor);
            console.SetChar(x + width, y, Glyph.BoxTopRight);
            console.SetColor(x + width, y + height, borderColor);
            console.SetChar(x + width, y + height, Glyph.BoxBottomRight);

            for (int i = x + 1; i < x + width; i++)
            {
                console.SetColor(i, y, borderColor);
                console.SetChar(i, y, Glyph.BoxHorizontalLine);
                console.SetColor(i, y + height, borderColor);
                console.SetChar(i, y + height, Glyph.BoxHorizontalLine);
            }

            for (int j = y + 1; j < y + height; j++)
            {
                console.SetColor(x, j, borderColor);
                console.SetChar(x, j, Glyph.BoxVerticalLine);
                console.SetColor(x + width, j, borderColor);
                console.SetChar(x + width, j, Glyph.BoxVerticalLine);
            }

        }
    }
}
