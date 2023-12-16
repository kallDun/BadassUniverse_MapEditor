using System.Collections.Generic;
using System.Linq;
using MapEditor.Services.Properties.Attributes;
using Newtonsoft.Json;

namespace MapEditor.Models.Server
{
    public class WorldDTO : AItemDTO
    {
        [JsonProperty("id"), CustomProperty(isReadOnly: true)] public required int Id { get; set; } 
        [JsonProperty("name"), CustomProperty] public required string Name { get; set; }
        [JsonProperty("xLength"), CustomProperty("Length")] public required int XLenght { get; set; }
        [JsonProperty("yLength"), CustomProperty("Height")] public required int YLenght { get; set; }
        [JsonProperty("playerSpawnRoomId"), CustomProperty("Spawn Room Id")] public required int PlayerSpawnRoomId { get; set; }
        [JsonProperty("version"), CustomProperty(isReadOnly: true)] public required string Version { get; set; }
        [JsonProperty("rooms")] public List<RoomDTO> Rooms { get; set; } = new();
        [JsonProperty("facades")] public List<FacadeDTO> Facades { get; set; } = new();
        
        public override object Clone()
        {
            return new WorldDTO
            {
                Id = Id,
                Name = Name,
                XLenght = XLenght,
                YLenght = YLenght,
                PlayerSpawnRoomId = PlayerSpawnRoomId,
                Rooms = Rooms.Select(x => (RoomDTO)x.Clone()).ToList(),
                Facades = Facades.Select(x => (FacadeDTO)x.Clone()).ToList(),
                Version = Version,
                State = State
            };
        }
    }
}
