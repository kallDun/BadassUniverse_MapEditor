using System.Windows.Media;
using MapEditor.Models.Game.Data;

namespace MapEditor.Models.Game.Rooms
{
    public class AnyFormRoom : Room
    {
        private readonly MapDirection rotation;
        private readonly AnyFormBuildingParameters parameters;
        private readonly RoomDoorParameters[] doorParameters;

        public AnyFormRoom(int id, string name, int floor, Color color, MapDirection rotation, StoredPreviewState state, 
            AnyFormBuildingParameters parameters, RoomDoorParameters[] doorParameters) 
            : base(id, name, floor, color, state)
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
                map.RoomInit_FillSquareRoomSpace(Id, 
                    new MapIndex(0, 0), 
                    new MapIndex(squareRoom.Width + 1, squareRoom.Length + 1),
                    Floor, State);
            }
            
            foreach (var door in doorParameters)
            {
                map.RoomInit_SetDoor(Id, door.Position, door.Id, door.FloorDisplacement, Floor, State);
            }
            map = map.RotateMap(rotation);
            return map;
        }
    }
}
