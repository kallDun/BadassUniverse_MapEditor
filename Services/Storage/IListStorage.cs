using System.Collections.Generic;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Storage;

public interface IListStorage
{
    IEnumerable<RoomDTO> GetRooms(IGameStorage storage);
}