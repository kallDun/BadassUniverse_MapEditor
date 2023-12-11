using MapEditor.Services.Properties.Attributes;
using Newtonsoft.Json;

namespace MapEditor.Models.Game.Concrete.Rooms
{
    public class StreetRoomParameters
    {
        [JsonProperty("width"), CustomProperty] public int Width { get; set; }
        [JsonProperty("length"), CustomProperty] public int Length { get; set; }
    }
}
