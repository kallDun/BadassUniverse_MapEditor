using System;
using System.Windows.Media;

namespace BadassUniverse_MapEditor.Models.Game.Concrete.Rooms
{
    public class StreetRoom : Room
    {
        private MapDirection rotation;
        private readonly StreetRoomParameters parameters;
        private readonly StreetRoomDoorParameters[] doorParameters;
        private int nextDoorIndex = 0;

        public StreetRoom(int id, string name, int floor, Color color, MapDirection rotation, 
            StreetRoomParameters parameters, StreetRoomDoorParameters[] doorParameters) 
            : base(id, name, floor, color)
        {
            this.rotation = rotation;
            this.parameters = parameters;
            this.doorParameters = doorParameters;
        }

        protected override Map GenerateMap()
        {
            Map map = Map.InitMap(parameters.Width + 2, parameters.Length + 2);
            map.RoomInit_FillSquareRoomSpace(Id, new MapIndex(0, 0), new MapIndex(parameters.Width + 1, parameters.Length + 1));
            foreach (var door in doorParameters)
            {
                GenerateDoor(door, map);
            }
            map = map.RotateMap(rotation);
            return map;
        }

        private void GenerateDoor(StreetRoomDoorParameters doorParameters, Map map)
        {
            if (doorParameters.StartIndex < 0) 
                throw new ArgumentOutOfRangeException(nameof(doorParameters.StartIndex));
            if (doorParameters.Direction == MapDirection.Up || doorParameters.Direction == MapDirection.Down)
            {
                if (doorParameters.StartIndex + doorParameters.Length > parameters.Length)
                    throw new ArgumentOutOfRangeException(nameof(doorParameters.Length));
            }
            else
            {
                if (doorParameters.StartIndex + doorParameters.Length > parameters.Width)
                    throw new ArgumentOutOfRangeException(nameof(doorParameters.Length));
            }

            for (int i = doorParameters.StartIndex; i < doorParameters.StartIndex + doorParameters.Length; i++)
            {
                var MapIndex = doorParameters.Direction switch
                {
                    MapDirection.Up => new MapIndex(0, i + 1),
                    MapDirection.Down => new MapIndex(parameters.Width + 1, i + 1),
                    MapDirection.Left => new MapIndex(i + 1, 0),
                    MapDirection.Right => new MapIndex(i + 1, parameters.Length + 1),
                    _ => throw new ArgumentOutOfRangeException(nameof(doorParameters.Direction)),
                };
                map.GetValue(MapIndex).AddDoor(nextDoorIndex, Id, 0, Floor);
                nextDoorIndex++;
            }
        }
    }
}
