using System;

namespace BadassUniverse_MapEditor.Models.Game
{
    public class Map : ICloneable
    {
        public required MapCell[][] Array { get; set; }

        public Map RotateMap(MapDirection Rotation)
        {
            if (MapDirection.Up == Rotation) return this;

            Map newMap;

            if (MapDirection.Down == Rotation)
            {
                newMap = InitMap(GetSizeY(), GetSizeX());
                for (int i = 0; i < GetSizeY(); i++) 
                {
                    for (int j = 0; j < GetSizeX(); j++)
                    {
                        MapIndex Index = GetRotatedIndex(new MapIndex(i, j), Rotation, GetSizeX(), GetSizeY());
                        newMap.SetValue(Index, (MapCell)GetValue(new MapIndex(i, j)).Clone());
                    }
                }
            }
            else
            {
                newMap = InitMap(GetSizeX(), GetSizeY());
                for (int i = 0; i < GetSizeY(); i++)
                {
                    for (int j = 0; j < GetSizeX(); j++)
                    {
                        MapIndex Index = GetRotatedIndex(new MapIndex(i, j), Rotation, GetSizeY(), GetSizeX());
                        newMap.SetValue(Index, (MapCell)GetValue(new MapIndex(i, j)).Clone());
                    }
                }
            }

            return newMap;
        }

        private static MapIndex GetRotatedIndex(MapIndex MapIndex, MapDirection Rotation, int GetSizeY, int GetSizeX)
	    {
            return Rotation switch
            {
                MapDirection.Up => MapIndex,
                MapDirection.Right => new MapIndex(MapIndex.X, GetSizeY - MapIndex.Y - 1),
                MapDirection.Down => new MapIndex(GetSizeX - MapIndex.X - 1, GetSizeY - MapIndex.Y - 1),
                MapDirection.Left => new MapIndex(GetSizeX - MapIndex.X - 1, MapIndex.Y),
                _ => MapIndex,
            };
        }

        public bool FillWithInnerMap(Map InnerMap, int InnerMapFloorIndex, MapIndex TopLeftCorner, out Map OutGlobalMap)
	    {
            OutGlobalMap = (Map)Clone();

		    for (int i = 0; i < InnerMap.GetSizeY(); i++)
		    {
			    for (int j = 0; j < InnerMap.GetSizeX(); j++)
			    {
				    MapIndex LocalIndex = new(i, j);
                    MapIndex GlobalIndex = LocalIndex + TopLeftCorner;
                    MapCell InnerCell = (MapCell)InnerMap.GetValue(LocalIndex).Clone();

                    bool bFillResult = OutGlobalMap.GetValue(GlobalIndex).TryToAddInnerCell(InnerCell, InnerMapFloorIndex);
				    if (!bFillResult)
				    {
					    return false;
				    }
			    }
            }
            return true;
	    }

        public int GetSizeX() => Array[0].Length;

        public int GetSizeY() => Array.Length;

        public MapCell GetValue(MapIndex Index) => Array[Index.Y][Index.X];

        public void SetValue(MapIndex Index, MapCell Value) 
            => Array[Index.Y][Index.X] = Value;

        public bool IsIndexValid(MapIndex Index)
            => Index.X >= 0 && Index.X < GetSizeX() && Index.Y >= 0 && Index.Y < GetSizeY();

        public MapIndex GetCenterLocation() => new(GetSizeY() / 2, GetSizeX() / 2);

        public static Map InitMap(int SizeY, int SizeX)
        {
            Map map = new() { Array = new MapCell[SizeY][] };
            for (int i = 0; i < SizeY; i++)
            {
                map.Array[i] = new MapCell[SizeX];
                for (int j = 0; j < SizeX; j++)
                {
                    map.Array[i][j] = MapCell.InitEmpty();
                }
            }
            return map;
        }

        public object Clone()
        {
            Map NewMap = InitMap(GetSizeY(), GetSizeX());
            for (int i = 0; i < GetSizeY(); i++)
            {
                for (int j = 0; j < GetSizeX(); j++)
                {
                    NewMap.SetValue(new MapIndex(i, j), (MapCell)GetValue(new MapIndex(i, j)).Clone());
                }
            }
            return NewMap;
        }

        public void RoomInit_FillSquareRoomSpace(int RoomIndex, MapIndex TopLeftCornerWithWalls, MapIndex BottomRightCornerWithWalls)
        {
            for (int i = TopLeftCornerWithWalls.Y; i <= BottomRightCornerWithWalls.Y; i++)
            {
                for (int j = TopLeftCornerWithWalls.X; j <= BottomRightCornerWithWalls.X; j++)
                {
                    var Cell = MapCell.InitEmpty();

                    if (i == TopLeftCornerWithWalls.Y || i == BottomRightCornerWithWalls.Y
                        || j == TopLeftCornerWithWalls.X || j == BottomRightCornerWithWalls.X)
                    {
                        Cell.AddWall(RoomIndex);
                    }
                    else
                    {
                        Cell.AddRoom(RoomIndex);
                    }

                    SetValue(new MapIndex(i, j), Cell);
                }
            }
        }

        public void RoomInit_SetDoor(int RoomIndex, MapIndex Index, int DoorIndex, int FloorDisplacement)
        {
            var Cell = MapCell.InitEmpty();
            Cell.AddDoor(DoorIndex, RoomIndex, FloorDisplacement);
            SetValue(Index, Cell);
        }

        public void BuildingInit_FillSquareBuildingSpace(int BuildingIndex, MapIndex TopLeftCorner, MapIndex BottomRightCorner)
        {
            for (int i = TopLeftCorner.Y; i <= BottomRightCorner.Y; i++)
            {
                for (int j = TopLeftCorner.X; j <= BottomRightCorner.X; j++)
                {
                    var Cell = MapCell.InitEmpty();
                    Cell.AddBuilding(BuildingIndex);
                    SetValue(new MapIndex(i, j), Cell);
                }
            }
        }

    }
}
