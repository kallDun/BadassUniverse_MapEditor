using System.Collections.Generic;
using MapEditor.Models.Server;

namespace MapEditor.Services.Storage;

public class TestListStorage : IListStorage
{
    public IEnumerable<RoomDTO> GetRooms(IGameStorage storage)
    {
        List <RoomDTO> rooms = new()
        {
            new RoomDTO
            {
                InGameRoomId = 0,
                Name = storage.GetRoomData(0)?.Name 
            }
        };
        return rooms;
    }
}
