using MapEditor.Models.Server.Icons;
using MapEditor.Services.Properties.Attributes;
using Newtonsoft.Json;

namespace MapEditor.Models.Server;

public class MobDTO : AItemDTO
{
    [JsonIgnore, CustomProperty(isReadOnly: true)] public int Id { get; set; }
    [JsonProperty("roomId"), CustomProperty("Room Id", true)] public int RoomId { get; set; }
    [JsonProperty("inGameMobId")] public int InGameMobId { get; set; }
    [JsonProperty("name"), CustomProperty] public required string Name { get; set; } = "Mob";
    [JsonProperty("color"), CustomProperty] public ColorDTO Color { get; set; }
    [JsonProperty("icon"), CustomProperty] public MobIconTypeDTO Icon { get; set; } = 0;
    [JsonProperty("roomOffsetX"), CustomProperty("X")] public int RoomOffsetX { get; set; }
    [JsonProperty("roomOffsetY"), CustomProperty("Y")] public int RoomOffsetY { get; set; }
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
            Color = Color,
            Icon = Icon,
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