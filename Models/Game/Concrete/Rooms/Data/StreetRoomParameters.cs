using MapEditor.Services.Properties.Attributes;

namespace MapEditor.Models.Game.Concrete.Rooms
{
    public class StreetRoomParameters
    {
        [CustomProperty] public int Width { get; set; }
        [CustomProperty] public int Length { get; set; }
    }
}
