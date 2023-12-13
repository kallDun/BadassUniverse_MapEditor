using System.Collections.Generic;
using System.Linq;
using MapEditor.Services.Storage.Data;

namespace MapEditor.Services.Storage
{
    public interface IGameStorage
    {
        IEnumerable<RoomStorageData> GetRoomsData();
        
        IEnumerable<FacadeStorageData> GetFacadesData();

        RoomStorageData? GetRoomData(int id)
        {
            return GetRoomsData().FirstOrDefault(room => room.Id == id);
        }
        
        FacadeStorageData? GetFacadeData(int id)
        {
            return GetFacadesData().FirstOrDefault(facade => facade.Id == id);
        }
        
    }
}
