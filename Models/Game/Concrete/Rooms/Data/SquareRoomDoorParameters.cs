namespace MapEditor.Models.Game.Concrete.Rooms
{
    public class SquareRoomDoorParameters
    {
        public int Id { get; set; }
        public MapIndex Position { get; set; }
        public int FloorDisplacement { get; set; }
    }
}
