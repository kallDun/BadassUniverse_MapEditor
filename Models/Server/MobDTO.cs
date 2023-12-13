using MapEditor.Services.Properties.Attributes;
using Newtonsoft.Json;

namespace MapEditor.Models.Server
{
    public class MobDTO : AItemDTO
    {
        [JsonProperty("id"), CustomProperty(isReadOnly: true)] public int Id { get; set; }
        [JsonProperty("roomId"), CustomProperty("Room Id", true)] public int RoomId { get; set; }
        [JsonProperty("inGameMobId")] public int InGameMobId { get; set; }
        [JsonProperty("name"), CustomProperty] public required string Name { get; set; } = "Mob";
        [JsonProperty("roomOffsetX"), CustomProperty("X")] public float RoomOffsetX { get; set; }
        [JsonProperty("roomOffsetY"), CustomProperty("Y")] public float RoomOffsetY { get; set; }
        [JsonProperty("isRotationRandom"), CustomProperty("Rotation Random")] public bool IsRotationRandom { get; set; }
        [JsonProperty("zAngleRotation"), CustomProperty("Rotation", showIfProperty: "IsRotationRandom", showIfValue: false)] public float ZAngleRotation { get; set; }
        [JsonProperty("spawnRadius"), CustomProperty("Spawn Radius")] public int SpawnRadius { get; set; }
        [JsonProperty("spawnTryCount"), CustomProperty("Spawn Tries")] public int SpawnTryCount { get; set; }
        
        public override object Clone()
        {
            return new MobDTO
            {
                Id = Id,
                RoomId = RoomId,
                InGameMobId = InGameMobId,
                Name = Name,
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
