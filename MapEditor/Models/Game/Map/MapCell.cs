using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace MapEditor.Models.Game
{
    public class MapCell : ICloneable
    {
        private Dictionary<int, MapCellFloor> Floors { get; set; } = new();

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
        public bool AddRoom(int roomIndex, int floorIndex, Color color)
        {
            return GetFloor(floorIndex).AddRoom(roomIndex, color);
        }
        public bool AddWall(int relatedRoomIndex, int floorIndex, Color color)
        {
            return GetFloor(floorIndex).AddWall(relatedRoomIndex, color);
        }
        public bool AddDoor(int doorIndex, int relatedRoomIndex, Color color, int roomFloorDisplacement = 0, int floorIndex = 0)
        {
            return GetFloor(floorIndex).AddDoor(doorIndex, relatedRoomIndex, color, roomFloorDisplacement);
        }
        public bool AddBuilding(int buildingIndex, int floorIndex, Color color)
        {
            return GetFloor(floorIndex).AddBuilding(buildingIndex, color);
        }

        public MapItemRoom? GetRoom(int floorIndex)
        {
            return GetFloor(floorIndex).GetRoom();
        }
        public List<MapItemWall> GetWalls(int floorIndex)
        {
            return GetFloor(floorIndex).GetWalls();
        }
        public List<MapItemDoor> GetDoors(int floorIndex)
        {
            return GetFloor(floorIndex).GetDoors();
        }
        public List<MapItemBuilding> GetBuilding(int floorIndex)
        {
            return GetFloor(floorIndex).GetBuildings();
        }

        private MapCellFloor GetFloor(int floorIndex)
        {
            if (!Floors.ContainsKey(floorIndex))
            {
                Floors.Add(floorIndex, new MapCellFloor());
            }
            return Floors[floorIndex];
        }

        public static MapCell InitEmpty() => new();

        public bool TryToAddInnerCell(MapCell InnerMapCell, int FloorIndex)
        {
            var Floor = GetFloor(FloorIndex);
            var InnerCellFloor = InnerMapCell.GetFloor(FloorIndex);
            if (InnerCellFloor.IsEmpty()) return false;

            var Room = InnerCellFloor.GetRoom();
            if (Room is not null)
		    {
                bool bAddResult = Floor.AddRoom(Room.Index, Room.Color);
                if (!bAddResult) return false;
            }

            var Walls = InnerCellFloor.GetWalls();
            if (Walls.Count > 0)
		    {
                foreach (var Wall in Walls)
			    {
                    bool bAddResult = Floor.AddWall(Wall.RelatedRoomIndex, Wall.Color);
                    if (!bAddResult) return false;
                }
            }

            var Doors = InnerCellFloor.GetDoors();
            if (Doors.Count > 0)
            {   
                foreach (var Door in Doors)
			    {
                    bool bAddResult = Floor.AddDoor(Door.DoorIndex, Door.RelatedRoomIndex, Door.Color, Door.RoomFloorDisplacement);
                    if (!bAddResult) return false;
                }
            }

            var Buildings = InnerCellFloor.GetBuildings();
            if (Buildings.Count > 0)
		    {
                foreach (var Building in Buildings)
			    {
                    bool bAddResult = Floor.AddBuilding(Building.Index, Building.Color);
                    if (!bAddResult) return false;
                }
            }

            var state = (StoredPreviewState)Math.Max((int)InnerCellFloor.State, (int)GetState(FloorIndex));
            SetState(state, FloorIndex);

            return true;
        }
        
        public object Clone()
        {
            Dictionary<int, MapCellFloor> FloorsCopy = new();
            foreach (var Floor in Floors)
            {
                FloorsCopy.Add(Floor.Key, (MapCellFloor)Floor.Value.Clone());
            }
            MapCell NewCell = InitEmpty();
            NewCell.Floors = FloorsCopy;
            return NewCell;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var Floor in Floors)
            {
                hash += Floor.Key;
                hash += Floor.Value.GetHashCode();
            }
            return hash;
        }

        public void SetState(StoredPreviewState state, int floor)
        {
            GetFloor(floor).SetState(state);
        }

        public StoredPreviewState GetState(int currentFloor)
        {
            return GetFloor(currentFloor).State;
        }
    }
}
