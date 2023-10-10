using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadassUniverse_MapEditor.Models.Game
{
    public class MapCell : ICloneable
    {
        public Dictionary<int, MapCellFloor> Floors { get; set; } = new();

        public bool IsEmpty()
        {
            foreach (var floor in Floors)
            {
                if (!floor.Value.IsEmpty())
                {
                    return false;
                }
            }
            return true;
        }
        public bool AddRoom(int roomIndex, int floorIndex = 0)
        {
            return GetFloor(floorIndex).AddRoom(roomIndex);
        }
        public bool AddWall(int relatedRoomIndex, int floorIndex = 0)
        {
            return GetFloor(floorIndex).AddRoom(relatedRoomIndex);
        }
        public bool AddDoor(int doorIndex, int relatedRoomIndex, int roomFloorDisplacement = 0, int floorIndex = 0)
        {
            return GetFloor(floorIndex).AddDoor(doorIndex, relatedRoomIndex, roomFloorDisplacement);
        }
        public bool AddBuilding(int buildingIndex, int floorIndex = 0)
        {
            return GetFloor(floorIndex).AddBuilding(buildingIndex);
        }

        public MapItemRoom GetRoom(int floorIndex)
        {
            return GetFloor(floorIndex).GetRoom();
        }
        public List<MapItemWall> GetWalls(int floorIndex = 0)
        {
            return GetFloor(floorIndex).GetWalls();
        }
        public List<MapItemDoor> GetDoors(int floorIndex = 0)
        {
            return GetFloor(floorIndex).GetDoors();
        }
        public List<MapItemBuilding> GetBuilding(int floorIndex = 0)
        {
            return GetFloor(floorIndex).GetBuildings();
        }

        public bool TryToADDInnerCell(MapCell innerMapCell, int floorIndex)
        {
            var floor = GetFloor(floorIndex);
            var innerCellFloor = innerMapCell.GetFloor(floorIndex);
            if (innerCellFloor.IsEmpty()) return false;

            var room = innerCellFloor.GetRoom();
            if (room != null)
            {
                var addResult = floor.AddRoom(room.Index);
                if (!addResult)
                {
                    return false;
                }
            }

            var walls = innerCellFloor.GetWalls();
            if (walls.Count() > 0)
            {
                foreach (var wall in walls)
                {
                    var addResult = floor.AddWall(wall.RelatedRoomIndex);
                    if (!addResult)
                    {
                        return false;
                    }
                }
            }

            var doors = innerCellFloor.GetDoors();
            if (doors.Count() > 0)
            {
                foreach (var door in doors)
                {
                    var addResult = floor.AddDoor(door.DoorIndex, door.RelatedRoomIndex, door.RoomFloorDisplacement);
                    if (!addResult)
                    {
                        return false;
                    }
                }
            }

            var buildings = innerCellFloor.GetBuildings();
            if (buildings.Count() > 0)
            {
                foreach (var building in buildings)
                {
                    var addResult = building.AddWall(building.Index);
                    if (!addResult)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public MapCellFloor GetFloor(int floorIndex)
        {
            if (!Floors.ContainsKey(floorIndex))
            {
                Floors.Add(floorIndex, new MapCellFloor());
            }
            return Floors[floorIndex];
        }
    }
}
