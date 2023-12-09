using BadassUniverse_MapEditor.Models.Game;
using System.Collections.Generic;
using System.Linq;
using BadassUniverse_MapEditor.Extensions.Attributes;
using BadassUniverse_MapEditor.Models.Game.Concrete.Rooms;
using Newtonsoft.Json;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class RoomDTO : AItemDTO
    {
        [Property(isReadOnly: true)] public required int Id { get; set; }
        public required int MapId { get; set; }
        public required int InGameRoomId { get; set; }
        [Property(isReadOnly: true)] public required string? Name { get; set; }
        [Property] public required ColorDTO Color { get; set; }
        [Property("Coordinate X", true)] public required int MapOffsetX { get; set; }
        [Property("Coordinate Y", true)] public required int MapOffsetY { get; set; }
        [Property] public required MapDirection Rotation { get; set; }
        [Property(isReadOnly: true)] public required int Floor { get; set; }
        [PropertyStringSerialized(types: new[] { typeof(StreetRoomParameters) })] public string? Params { get; set; }
        [PropertyStringSerialized(types: new[] { typeof(StreetRoomDoorParameters) })] public string? DoorParams { get; set; }
        public required List<PhysicsItemDTO> PhysicsItems { get; set; }
        public required List<MobDTO> Mobs { get; set; }
        
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
