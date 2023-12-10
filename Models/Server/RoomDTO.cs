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
        [CustomProperty(isReadOnly: true)] public int Id { get; set; }
        public int MapId { get; set; }
        public required int InGameRoomId { get; set; }
        [CustomProperty(isReadOnly: true)] public string? Name { get; set; }
        [CustomProperty] public ColorDTO Color { get; set; }
        [CustomProperty("X")] public int MapOffsetX { get; set; }
        [CustomProperty("Y")] public int MapOffsetY { get; set; }
        [CustomProperty] public MapDirection Rotation { get; set; }
        [CustomProperty(isReadOnly: true)] public int Floor { get; set; }
        [CustomPropertyStringSerialized(types: new[] { typeof(StreetRoomParameters) })] public string? Params { get; set; }
        [CustomPropertyStringSerialized(types: new[] { typeof(StreetRoomDoorParameters) })] public string? DoorParams { get; set; }
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
