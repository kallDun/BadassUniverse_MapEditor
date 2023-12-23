using System.Collections.Generic;
using System.Linq;
using MapEditor.Services.Storage.Data;

namespace MapEditor.Services.Storage;

public interface IGameStorage
{
    IEnumerable<RoomStorageData> GetRoomsData();
        
    IEnumerable<FacadeStorageData> GetFacadesData();
        
    IEnumerable<PhysicsItemStorageData> GetPhysicsItemsData();
        
    IEnumerable<MobStorageData> GetMobsData();

    RoomStorageData? GetRoomData(int id) => GetRoomsData().FirstOrDefault(room => room.Id == id);

    FacadeStorageData? GetFacadeData(int id) => GetFacadesData().FirstOrDefault(facade => facade.Id == id);

    PhysicsItemStorageData? GetPhysicsItemData(int id) => GetPhysicsItemsData().FirstOrDefault(physicsItem => physicsItem.Id == id);

    MobStorageData? GetMobData(int id) => GetMobsData().FirstOrDefault(mob => mob.Id == id);
}