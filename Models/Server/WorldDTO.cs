using System;
using System.Collections.Generic;
using System.Linq;

namespace MapEditor.Models.Server
{
    public class WorldDTO : ICloneable
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int XLenght { get; set; }
        public required int YLenght { get; set; }
        public required int PlayerSpawnRoomId { get; set; }
        public List<RoomDTO> Rooms { get; set; } = new();
        public List<FacadeDTO> Facades { get; set; } = new();
        public required string Version { get; set; }
        
        public object Clone()
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
                Version = Version
            };
        }
    }
}
