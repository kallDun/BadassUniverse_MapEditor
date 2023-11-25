using System.Collections.Generic;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class MapDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int XLenght { get; set; }
        public int YLenght { get; set; }
        public int PlayerSpawnRoomId { get; set; }
        public required List<RoomDTO> Rooms { get; set; }
        public required List<FacadeDTO> Facades { get; set; }
        public required string Version { get; set; }
    }
}
