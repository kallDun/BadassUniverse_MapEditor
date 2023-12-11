using System;
using System.Collections.Generic;
using MapEditor.Models.Game.Concrete.Rooms;
using MapEditor.Models.Server;
using Newtonsoft.Json;

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
                Name = storage.GetRoomData(0)?.Name ?? "Room"
            },
            new RoomDTO
            {
                InGameRoomId = 1,
                Name = storage.GetRoomData(1)?.Name ?? "Room",
                Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 15, Length = 10 }),
                DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
            },
        };
        return rooms;
    }
}
