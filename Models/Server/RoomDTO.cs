using System.Collections.Generic;
using System.Linq;
using MapEditor.Services.Properties.Attributes;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Concrete.Rooms;
using Newtonsoft.Json;

namespace MapEditor.Models.Server
{
    public class RoomDTO : AItemDTO
    {
        [JsonProperty("id"), CustomProperty(isReadOnly: true)] public int Id { get; set; }
        [JsonProperty("mapId")] public int MapId { get; set; }
        [JsonProperty("inGameRoomId")] public required int InGameRoomId { get; set; }
        [JsonProperty("name"), CustomProperty] public required string Name { get; set; }
        [JsonProperty("color"), CustomProperty] public ColorDTO Color { get; set; }
        [JsonProperty("mapOffsetX"), CustomProperty("X")] public int MapOffsetX { get; set; }
        [JsonProperty("mapOffsetY"), CustomProperty("Y")] public int MapOffsetY { get; set; }
        [JsonProperty("rotation"), CustomProperty] public MapDirection Rotation { get; set; }
        [JsonProperty("floor"), CustomProperty] public int Floor { get; set; }
        [JsonProperty("params"), CustomPropertyStringSerialized(types: new[] { typeof(StreetRoomParameters) })] public string? Params { get; set; }
        [JsonProperty("doorParams"), CustomPropertyStringSerialized(types: new[] { typeof(List<StreetRoomDoorParameters>) })] public string? DoorParams { get; set; }
        [JsonProperty("physicsItems"), CustomProperty] public List<PhysicsItemDTO> PhysicsItems { get; set; } = new();
        [JsonProperty("mobs"), CustomProperty] public List<MobDTO> Mobs { get; set; } = new();
        
        public override object Clone()
        {
            return new RoomDTO
            {
                Id = Id,
                MapId = MapId,
                InGameRoomId = InGameRoomId,
                Name = Name,
                Color = Color,
                MapOffsetX = MapOffsetX,
                MapOffsetY = MapOffsetY,
                Rotation = Rotation,
                Floor = Floor,
                Params = Params,
                DoorParams = DoorParams,
                PhysicsItems = PhysicsItems.Select(item => (PhysicsItemDTO)item.Clone()).ToList(),
                Mobs = Mobs.Select(mob => (MobDTO)mob.Clone()).ToList(),
                State = State
            };
        }
    }
}
