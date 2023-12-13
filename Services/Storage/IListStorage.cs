using System.Collections.Generic;
using MapEditor.Models.Server;

namespace MapEditor.Services.Storage;

public interface IListStorage
{
    IEnumerable<RoomDTO> GetRooms(IGameStorage storage);
    IEnumerable<FacadeDTO> GetFacades(IGameStorage storage);
    
}