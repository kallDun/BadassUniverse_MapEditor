namespace BadassUniverse_MapEditor.Models.Server
{
    public class PhysicsItemDTO
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int InGamePhysicsItemId { get; set; }
        public bool SimulatePhysics { get; set; }
        public float RoomOffsetX { get; set; }
        public float RoomOffsetY { get; set; }
        public bool IsRotationRandom { get; set; }
        public float ZAngleRotation { get; set; }
        public int SpawnRadius { get; set; }
        public int SpawnTryCount { get; set; }
    }
}
