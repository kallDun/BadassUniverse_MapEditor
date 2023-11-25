using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Game.Concrete.Rooms;
using BadassUniverse_MapEditor.Services.Storage.Data;
using System.Collections.Generic;
using System.Linq;

namespace BadassUniverse_MapEditor.Services.Storage
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
                }
            };
        }

        public RoomStorageData? GetRoomData(int id)
        {
            return GetRoomsData().FirstOrDefault(room => room.Id == id);
        }
    }
}
