using MapEditor.Services.Properties.Attributes;

namespace MapEditor.Models.Game.Concrete.Rooms
{
    public class StreetRoomDoorParameters
    {
        [CustomProperty] public MapDirection Direction { get; set; }
        [CustomProperty] public int StartIndex { get; set; }
        [CustomProperty] public int Length { get; set; }
        
        [CustomProperty] public StreetRoomParameters Parameters { get; set; } = new();
    }
}
