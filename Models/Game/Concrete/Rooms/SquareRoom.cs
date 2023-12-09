using System.Windows.Media;

namespace BadassUniverse_MapEditor.Models.Game.Concrete.Rooms
{
    public class SquareRoom : Room
    {
        private MapDirection rotation;
        private SquareRoomParameters parameters;
        private SquareRoomDoorParameters[] doorParameters;

        public SquareRoom(int id, string name, int floor, Color color, MapDirection rotation, StoredPreviewState state,
            SquareRoomParameters parameters, SquareRoomDoorParameters[] doorParameters) 
            : base(id, name, floor, color, state)
        {
            this.rotation = rotation;
            this.parameters = parameters;
            this.doorParameters = doorParameters;
        }

        protected override Map GenerateMap()
        {
            Map map = Map.InitMap(parameters.Width + 2, parameters.Length + 2);
            map.RoomInit_FillSquareRoomSpace(Id, 
                new MapIndex(0, 0), 
                new MapIndex(parameters.Width + 1, parameters.Length + 1),
                Floor, State);
            foreach (var door in doorParameters)
            {
                map.RoomInit_SetDoor(Id, door.Position, door.Id, door.FloorDisplacement, Floor, State);
            }
            map = map.RotateMap(rotation);
            return map;
        }
    }
}
