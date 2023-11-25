using System.Windows.Media;

namespace BadassUniverse_MapEditor.Models.Game.Concrete.Rooms
{
    public class AnyFormRoom : Room
    {
        private MapDirection rotation;
        private AnyFormRoomParameters parameters;
        private SquareRoomDoorParameters[] doorParameters;

        public AnyFormRoom(int id, string name, int floor, Color color, MapDirection rotation, 
            AnyFormRoomParameters parameters, SquareRoomDoorParameters[] doorParameters) 
            : base(id, name, floor, color)
        {
            this.rotation = rotation;
            this.parameters = parameters;
            this.doorParameters = doorParameters;
        }

        protected override Map GenerateMap()
        {
            Map map = Map.InitMap(parameters.Width + 2, parameters.Length + 2);
            foreach (var squareRoom in parameters.SquareRooms)
            {
                map.RoomInit_FillSquareRoomSpace(Id, new MapIndex(0, 0), new MapIndex(squareRoom.Width + 1, squareRoom.Length + 1));
            }
            
            foreach (var door in doorParameters)
            {
                map.RoomInit_SetDoor(Id, door.Position, door.Id, door.FloorDisplacement);
            }
            map = map.RotateMap(rotation);
            return map;
        }
    }
}
