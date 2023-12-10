using MapEditor.Models.Game;
using MapEditor.Services.Properties.Attributes;
using Newtonsoft.Json;

namespace MapEditor.Models.Server
{
    public class FacadeDTO : AItemDTO
    {
        [JsonProperty("id"), CustomProperty(isReadOnly: true)] public int Id { get; set; }
        [JsonProperty("mapId")] public int MapId { get; set; }
        [JsonProperty("inGameFacadeId")] public int InGameFacadeId { get; set; }
        [JsonProperty("name"), CustomProperty] public required string Name { get; set; }
        [JsonProperty("color"), CustomProperty] public ColorDTO Color { get; set; }
        [JsonProperty("mapOffsetX"), CustomProperty("X")] public int MapOffsetX { get; set; }
        [JsonProperty("mapOffsetY"), CustomProperty("Y")] public int MapOffsetY { get; set; }
        [JsonProperty("rotation"), CustomProperty] public MapDirection Rotation { get; set; }
        [JsonProperty("floor"), CustomProperty] public int Floor { get; set; }
        
        public override object Clone()
        {
            return new FacadeDTO
            {
                Id = Id,
                MapId = MapId,
                InGameFacadeId = InGameFacadeId,
                Name = Name,
                MapOffsetX = MapOffsetX,
                MapOffsetY = MapOffsetY,
                Color = Color,
                Floor = Floor,
                Rotation = Rotation,
                State = State
            };
        }
    }
}
