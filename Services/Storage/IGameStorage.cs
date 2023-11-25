using BadassUniverse_MapEditor.Services.Storage.Data;
using System.Collections.Generic;

namespace BadassUniverse_MapEditor.Services.Storage
{
    public interface IGameStorage
    {
        List<RoomStorageData> GetRoomsData();

        RoomStorageData? GetRoomData(int id);
    }
}
