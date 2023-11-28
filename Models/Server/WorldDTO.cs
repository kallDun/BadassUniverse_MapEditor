using System.Collections.Generic;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class WorldDTO
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required int XLenght { get; set; }
        public required int YLenght { get; set; }
        public required int PlayerSpawnRoomId { get; set; }
        public required List<RoomDTO> Rooms { get; set; }
        public required List<FacadeDTO> Facades { get; set; }
        public required string Version { get; set; }
    }
}
