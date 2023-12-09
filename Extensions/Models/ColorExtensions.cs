using System;
using System.Windows.Media;

namespace BadassUniverse_MapEditor.Extensions.Models;

public static class ColorExtensions
{
    public static Color GetRandomColor()
    {
        var random = new Random();
        var r = random.Next(0, 255);
        var g = random.Next(0, 255);
        var b = random.Next(0, 255);
        return Color.FromRgb((byte)r, (byte)g, (byte)b);
    }
}