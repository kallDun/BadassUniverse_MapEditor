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
        [Property(isReadOnly: true)] public int Id { get; set; }
        public int MapId { get; set; }
        public required int InGameRoomId { get; set; }
        [Property(isReadOnly: true)] public string? Name { get; set; }
        [Property] public ColorDTO Color { get; set; }
        [Property("Coordinate X", true)] public int MapOffsetX { get; set; }
        [Property("Coordinate Y", true)] public int MapOffsetY { get; set; }
        [Property] public MapDirection Rotation { get; set; }
        [Property(isReadOnly: true)] public int Floor { get; set; }
        [PropertyStringSerialized(types: new[] { typeof(StreetRoomParameters) })] public string? Params { get; set; }
        [PropertyStringSerialized(types: new[] { typeof(StreetRoomDoorParameters) })] public string? DoorParams { get; set; }
        public List<PhysicsItemDTO> PhysicsItems { get; set; } = new();
        public List<MobDTO> Mobs { get; set; } = new();
        
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
