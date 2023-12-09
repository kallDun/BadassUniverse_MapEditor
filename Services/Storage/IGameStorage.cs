using System.Collections.Generic;
using MapEditor.Services.Storage.Data;

namespace MapEditor.Services.Storage
{
    public interface IGameStorage
    {
        List<RoomStorageData> GetRoomsData();

        RoomStorageData? GetRoomData(int id);
    }
}
