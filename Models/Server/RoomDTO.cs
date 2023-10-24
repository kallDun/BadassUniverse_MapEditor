using System.Collections.Generic;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public int MapId { get; set; }
        public int InGameRoomId { get; set; }
        public int MapOffsetX { get; set; }
        public int MapOffsetY { get; set; }
        public string? Params { get; set; }
        public string? DoorParams { get; set; }
        public required List<PhysicsItemDTO> PhysicsItems { get; set; }
        public required List<MobDTO> Mobs { get; set; }
    }
}
