using Newtonsoft.Json;

namespace MapEditor.Models.Server
{
    public class PhysicsItemDTO : AItemDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int InGamePhysicsItemId { get; set; }
        public required string? Name { get; set; }
        public bool SimulatePhysics { get; set; }
        public float RoomOffsetX { get; set; }
        public float RoomOffsetY { get; set; }
        public bool IsRotationRandom { get; set; }
        public float ZAngleRotation { get; set; }
        public int SpawnRadius { get; set; }
        public int SpawnTryCount { get; set; }
        
        public override object Clone()
        {
            return new PhysicsItemDTO
            {
                Id = Id,
                RoomId = RoomId,
                InGamePhysicsItemId = InGamePhysicsItemId,
                Name = Name,
                SimulatePhysics = SimulatePhysics,
                RoomOffsetX = RoomOffsetX,
                RoomOffsetY = RoomOffsetY,
                IsRotationRandom = IsRotationRandom,
                ZAngleRotation = ZAngleRotation,
                SpawnRadius = SpawnRadius,
                SpawnTryCount = SpawnTryCount,
                State = State
            };
        }
    }
}
