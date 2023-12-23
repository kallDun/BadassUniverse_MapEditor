using System.Windows.Media;
using Newtonsoft.Json;

namespace MapEditor.Models.Server
{
    public struct ColorDTO
    {
        [JsonProperty("red")] public byte Red { get; set; }
        [JsonProperty("green")] public byte Green { get; set; }
        [JsonProperty("blue")] public byte Blue { get; set; }

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
