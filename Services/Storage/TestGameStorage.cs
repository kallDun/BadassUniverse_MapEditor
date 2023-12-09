using System.Collections.Generic;
using System.Linq;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Concrete.Rooms;
using MapEditor.Services.Storage.Data;

namespace MapEditor.Services.Storage
{
    public class TestGameStorage : IGameStorage
    {
        public List<RoomStorageData> GetRoomsData()
        {
            return new List<RoomStorageData>
            {
                new()
                {
                    Id = 0,
                    Name = "Test Square Room",
                    Params = new SquareRoomParameters
                    {
                        Width = 10,
                        Length = 10,
                    },
                    DoorParams = new SquareRoomDoorParameters[]
                    {
                        new()
                        {
                            Id = 0,
                            Position = new MapIndex(5, 0),
                            FloorDisplacement = 0
                        }
                    },
                    RoomType = typeof(SquareRoom)
                },
                new()
                {
                    Id = 1,
                    Name = "Room #2",
                    Params = new SquareRoomParameters
                    {
                        Width = 16,
                        Length = 16,
                    },
                    DoorParams = new SquareRoomDoorParameters[]
                    {
                        new()
                        {
                            Id = 0,
                            Position = new MapIndex(5, 0),
                            FloorDisplacement = 0
                        },
                        new()
                        {
                            Id = 1,
                            Position = new MapIndex(0, 10),
                            FloorDisplacement = 0
                        }
                    },
                    RoomType = typeof(SquareRoom)
                }
            };
        }

        public RoomStorageData? GetRoomData(int id)
        {
            return GetRoomsData().FirstOrDefault(room => room.Id == id);
        }
    }
}
