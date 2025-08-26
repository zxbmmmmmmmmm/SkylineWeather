using System.Numerics;
using Windows.UI;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;

namespace SkylineWeather.WinUI.Controls.Helpers;

internal static class CanvasExtensions
{
    public static void DrawCenteredTextLayout(this CanvasDrawingSession session, CanvasTextLayout textLayout, float x, float y, Color color)
    {
        var textHeight = (float)textLayout.DrawBounds.Height;
        var textWidth = (float)textLayout.DrawBounds.Width;
        session.DrawTextLayout(textLayout, new Vector2(x - textWidth / 2, y - textHeight), color);
    }
}