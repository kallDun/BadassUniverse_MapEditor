using Newtonsoft.Json;

namespace BadassUniverse_MapEditor.Models.Server
{
    public class MobDTO : AItemDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int InGameMobItemId { get; set; }
        public required string? Name { get; set; }
        public float RoomOffsetX { get; set; }
        public float RoomOffsetY { get; set; }
        public bool IsRotationRandom { get; set; }
        public float ZAngleRotation { get; set; }
        public int SpawnRotation { get; set; }
        public int SpawnTryCount { get; set; }
        
        public override object Clone()
        {
            return new MobDTO
            {
                Id = Id,
                RoomId = RoomId,
                InGameMobItemId = InGameMobItemId,
                Name = Name,
                RoomOffsetX = RoomOffsetX,
                RoomOffsetY = RoomOffsetY,
                IsRotationRandom = IsRotationRandom,
                ZAngleRotation = ZAngleRotation,
                SpawnRotation = SpawnRotation,
                SpawnTryCount = SpawnTryCount,
                State = State
            };
        }
    }
}
