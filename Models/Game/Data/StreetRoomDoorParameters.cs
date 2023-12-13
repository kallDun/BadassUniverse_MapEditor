using MapEditor.Services.Properties.Attributes;
using Newtonsoft.Json;

namespace MapEditor.Models.Game.Data
{
    public class StreetRoomDoorParameters
    {
        [JsonProperty("direction"), CustomProperty] public MapDirection Direction { get; set; }
        [JsonProperty("startIndex"), CustomProperty("Start Index")] public int StartIndex { get; set; }
        [JsonProperty("length"), CustomProperty] public int Length { get; set; }
    }
}
