using System;
using System.Collections.Generic;
using System.Linq;
using MapEditor.Models.Game;

namespace MapEditor.Extensions.Game
{
    public static class FindElementsFromWorldByCellExtension
    {
        public static Room? GetRoomFromWorldByCell(this World world, MapCell mapCell, int currentFloor)
        {
            MapItemRoom? itemRoom = mapCell.GetRoom(currentFloor);
            if (itemRoom != null)
            {
                return world.Rooms.FirstOrDefault(x => x.Id == itemRoom.Index);
            }
            return null;
        }

        public static Room? GetRoomFromWall(this World world, MapItemWall wall)
        {
            return world.Rooms.FirstOrDefault(x => x.Id == wall.RelatedRoomIndex);
        }

        public static Room? GetRoomFromDoor(this World world, MapItemDoor door)
        {
            return world.Rooms.FirstOrDefault(x => x.Id == door.RelatedRoomIndex);
        }

        public static IEnumerable<Facade> GetFacadesFromWorldByCell(this World world, MapCell mapCell, int currentFloor)
        {
            List<MapItemBuilding> buildings = mapCell.GetBuilding(currentFloor);
            foreach (MapItemBuilding building in buildings)
            {
                Facade? facade = world.Facades.FirstOrDefault(x => x.Id == building.Index);
                if (facade != null)
                {
                    yield return facade;
                }
            }
        }

        public static IEnumerable<MapIndex> GetCellsOfRoom(this World world, Room room, int currentFloor)
        {
            for (var x = 0; x < world.Map.GetSizeY(); x++)
            {
                for (var y = 0; y < world.Map.GetSizeY(); y++)
                {
                    MapIndex index = new MapIndex(y, x);
                    int? findRoomIndex = world.Map.GetValue(index).GetRoom(currentFloor)?.Index;
                    if (findRoomIndex == room.Id)
                    {
                        yield return index;
                    }
                }
            }
        }

        public static IEnumerable<MapIndex> GetClosestNeightborPositionsToRoom(this World world, Room room, MapIndex position, int currentFloor)
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var y = -1; y <= 1; y++)
                {
                    MapIndex index = new(position.Y + y, position.X + x);
                    if (world.Map.IsIndexValid(index) == false) continue;
                    int? findRoomIndex = world.Map.GetValue(index).GetRoom(currentFloor)?.Index;
                    if (findRoomIndex == room.Id)
                    {
                        yield return index;
                    }
                }
            }
        }
        
        public static (MapItemDoor Door, Room Room, int Floor)? GetDoorRelation(this World world, MapItemDoor door, MapIndex position, int currentFloor)
        {
            var floor = currentFloor + door.RoomFloorDisplacement;
            var doorResult = world.Map.GetValue(position).GetDoors(floor).FirstOrDefault(x => x != door);
            if (doorResult == null) return null;
            var room = world.GetRoomFromDoor(doorResult);
            if (room == null) return null;
            return (doorResult, room, floor);
        }
    }
}
