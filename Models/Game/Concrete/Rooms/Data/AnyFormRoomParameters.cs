namespace MapEditor.Models.Game.Concrete.Rooms
{
    public class AnyFormRoomParameters
    {
        public int Width { get; set; }
        public int Length { get; set; }

        public required SquareRoomParameters[] SquareRooms { get; set; }
    }
}
