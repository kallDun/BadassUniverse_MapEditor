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
                    Name = "Square Room",
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
                    Name = "Street Room",
                    RoomType = typeof(StreetRoom)
                }
            };
        }

        public RoomStorageData? GetRoomData(int id)
        {
            return GetRoomsData().FirstOrDefault(room => room.Id == id);
        }
    }
}
