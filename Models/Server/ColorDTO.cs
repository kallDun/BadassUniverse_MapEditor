using System.Windows.Media;

namespace BadassUniverse_MapEditor.Models.Server
{
    public struct ColorDTO
    {
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public static implicit operator Color(ColorDTO color)
        {
            return Color.FromRgb(color.Red, color.Green, color.Blue);
        }

        public static implicit operator ColorDTO(Color color)
        {
            return new ColorDTO
            {
                Red = color.R,
                Green = color.G,
                Blue = color.B
            };
        }
    }
}
