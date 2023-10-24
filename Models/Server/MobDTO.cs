namespace BadassUniverse_MapEditor.Models.Server
{
    public class MobDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int InGameMobItemId { get; set; }
        public float RoomOffsetX { get; set; }
        public float RoomOffsetY { get; set; }
        public bool IsRotationRandom { get; set; }
        public float ZAngleRotation { get; set; }
        public int SpawnRotation { get; set; }
        public int SpawnTryCount { get; set; }
    }
}
