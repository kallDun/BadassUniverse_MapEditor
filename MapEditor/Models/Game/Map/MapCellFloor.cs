using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public class MapCellFloor : ICloneable
    {
        public List<MapItem> Items { get; set; } = new();
        public StoredPreviewState State { get; private set; } = StoredPreviewState.Stored;

        public bool IsEmpty() => Items.Count == 0;

        public bool AddRoom(int roomIndex, Color color)
        {
            if (GetRoom() != null) return false;
            Items.Add(new MapItemRoom(roomIndex, color));
            return true;
        }

        public bool AddWall(int relatedRoomIndex, Color color)
        {
            if (GetRoom() != null) return false;
            Items.Add(new MapItemWall(relatedRoomIndex, color));
            return true;
        }

        public bool AddBuilding(int buildingIndex, Color color)
        {
            Items.Add(new MapItemBuilding(buildingIndex, color));
            return true;
        }

        public bool AddDoor(int doorIndex, int relatedRoomIndex, Color color, int roomFloorDisplacement = 0)
        {
            Items.Add(new MapItemDoor(doorIndex, relatedRoomIndex, roomFloorDisplacement, color));
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
            newCell.State = State;
            return newCell;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var item in Items)
            {
                hash += item.GetHashCode();
            }
            hash += State.GetHashCode() * 13;
            return hash;
        }

        public void SetState(StoredPreviewState state)
        {
            State = state;
        }
    }
}
