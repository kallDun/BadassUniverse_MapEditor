using System;
using System.Windows.Media;
using MapEditor.Models.Game.Data;

namespace MapEditor.Models.Game.Rooms
{
    public class StreetRoom : Room
    {
        private readonly MapDirection rotation;
        private readonly StreetRoomParameters parameters;
        private readonly StreetRoomDoorParameters[] doorParameters;
        private int nextDoorIndex = 0;

        public StreetRoom(int id, string name, MapIndex leftTopCorner, int floor, Color color, MapDirection rotation, StoredPreviewState state, 
            StreetRoomParameters parameters, StreetRoomDoorParameters[] doorParameters) 
            : base(id, name, leftTopCorner, floor, color, state)
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
                Floor, State, Color);
            foreach (var door in doorParameters)
            {
                GenerateDoor(door, map);
            }
            map = map.RotateMap(rotation);
            return map;
        }

        private void GenerateDoor(StreetRoomDoorParameters doorParams, Map map)
        {
            if (doorParams.StartIndex < 0) 
                throw new ArgumentOutOfRangeException(nameof(doorParams.StartIndex));
            if (doorParams.Direction == MapDirection.Up || doorParams.Direction == MapDirection.Down)
            {
                if (doorParams.StartIndex + doorParams.Length > parameters.Length)
                    throw new ArgumentOutOfRangeException(nameof(doorParams.Length));
            }
            else
            {
                if (doorParams.StartIndex + doorParams.Length > parameters.Width)
                    throw new ArgumentOutOfRangeException(nameof(doorParams.Length));
            }

            for (int i = doorParams.StartIndex; i < doorParams.StartIndex + doorParams.Length; i++)
            {
                var MapIndex = doorParams.Direction switch
                {
                    MapDirection.Up => new MapIndex(0, i + 1),
                    MapDirection.Down => new MapIndex(parameters.Width + 1, i + 1),
                    MapDirection.Left => new MapIndex(i + 1, 0),
                    MapDirection.Right => new MapIndex(i + 1, parameters.Length + 1),
                    _ => throw new ArgumentOutOfRangeException(nameof(doorParams.Direction)),
                };
                map.GetValue(MapIndex).AddDoor(nextDoorIndex, Id, Color, 0, Floor);
                nextDoorIndex++;
            }
        }
    }
}
