using System;
using System.Collections.Generic;

namespace BadassUniverse_MapEditor.Models.Game
{
    public class MapCellFloor : ICloneable
    {
        public List<MapItem> Items { get; set; } = new();

        public bool IsEmpty() => Items.Count == 0;

        public bool AddRoom(int roomIndex)
        {
            if (GetRoom() != null) return false;
            Items.Add(new MapItemRoom(roomIndex));
            return true;
        }

        public bool AddWall(int relatedRoomIndex)
        {
            if (GetRoom() != null) return false;
            Items.Add(new MapItemWall(relatedRoomIndex));
            return true;
        }

        public bool AddBuilding(int buildingIndex)
        {
            Items.Add(new MapItemBuilding(buildingIndex));
            return true;
        }

        public bool AddDoor(int doorIndex, int relatedRoomIndex, int roomFloorDisplacement = 0)
        {
            Items.Add(new MapItemDoor(doorIndex, relatedRoomIndex, roomFloorDisplacement));
            return true;
        }

        public MapItemRoom? GetRoom()
        {
            foreach (var item in Items)
            {
                if (item is MapItemRoom room) return room;
            }
            return null;
        }

        public List<MapItemWall> GetWalls()
        {
            List<MapItemWall> walls = new();
            foreach (var item in Items)
            {
                if (item is MapItemWall wall)
                {
                    walls.Add(wall);
                }
            }
            return walls;
        }

        public List<MapItemDoor> GetDoors()
        {
            List<MapItemDoor> doors = new();
            foreach (var item in Items)
            {
                if (item is MapItemDoor door)
                {
                    doors.Add(door);
                }
            }
            return doors;
        }

        public List<MapItemBuilding> GetBuildings()
        {
            List<MapItemBuilding> buildings = new();
            foreach (var item in Items)
            {
                if (item is MapItemBuilding building)
                {
                    buildings.Add(building);
                }
            }
            return buildings;
        }

        public object Clone()
        {
            MapCellFloor newCell = new MapCellFloor();
            List<MapItem> list = new List<MapItem>();

            foreach (var item in Items)
            {
                list.Add((MapItem)item.Clone());
            }
            newCell.Items = list;
            return newCell;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var item in Items)
            {
                hash += item.GetHashCode();
            }
            return hash;
        }
    }
}
