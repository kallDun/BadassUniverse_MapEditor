using System.Collections.Generic;
using MapEditor.Models.Server;

namespace MapEditor.Services.Storage;

public interface IListStorage
{
    IEnumerable<RoomDTO> GetRooms(IGameStorage storage);
    IEnumerable<FacadeDTO> GetFacades(IGameStorage storage);
    IEnumerable<PhysicsItemDTO> GetPhysicsItems(IGameStorage storage);
    IEnumerable<MobDTO> GetMobs(IGameStorage storage);
    
}