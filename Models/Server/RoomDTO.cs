using BadassUniverse_MapEditor.Models.Game;
using System.Collections.Generic;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class RoomDTO
    {
        public required int Id { get; set; }
        public required int MapId { get; set; }
        public required int InGameRoomId { get; set; }
        public required string? Name { get; set; }
        public required ColorDTO Color { get; set; }
        public required int MapOffsetX { get; set; }
        public required int MapOffsetY { get; set; }
        public required MapDirection Rotation { get; set; }
        public required int Floor { get; set; }
        public string? Params { get; set; }
        public string? DoorParams { get; set; }
        public required List<PhysicsItemDTO> PhysicsItems { get; set; }
        public required List<MobDTO> Mobs { get; set; }
    }
}
