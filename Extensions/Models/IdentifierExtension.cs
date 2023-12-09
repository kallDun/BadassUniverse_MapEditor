using System.Linq;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Extensions.Models;

public static class IdentifierExtension
{
    
    public static int GetNextRoomId(this WorldDTO worldDTO)
    {
        var maxId = worldDTO.Rooms.Select(room => room.Id).Prepend(0).Max();
        return maxId + 1;
    }
    
    public static int GetNextFacadeId(this WorldDTO worldDTO)
    {
        var maxId = worldDTO.Facades.Select(facade => facade.Id).Prepend(0).Max();
        return maxId + 1;
    }
    
    public static int GetNextPhysicsItemId(this RoomDTO roomDto)
    {
        var maxId = roomDto.PhysicsItems.Select(item => item.Id).Prepend(0).Max();
        return maxId + 1;
    }
    
    public static int GetNextMobId(this RoomDTO roomDto)
    {
        var maxId = roomDto.Mobs.Select(mob => mob.Id).Prepend(0).Max();
        return maxId + 1;
    }
    
}