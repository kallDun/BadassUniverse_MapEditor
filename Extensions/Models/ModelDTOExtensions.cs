using System.Linq;
using MapEditor.Models.Server;

namespace MapEditor.Extensions.Models;

public static class ModelDTOExtensions
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

    public static AItemDTO? FindItem(this WorldDTO worldDto, AItemDTO item)
    {
        if (item is WorldDTO world && world.Id == worldDto.Id) return worldDto;
        if (item is RoomDTO room) return worldDto.Rooms.Find(x => x.Id == room.Id);
        if (item is FacadeDTO facade) return worldDto.Facades.Find(x => x.Id == facade.Id);
        foreach (var roomItem in worldDto.Rooms)
        {
            switch (item)
            {
                case PhysicsItemDTO physicsItem when physicsItem.Id == roomItem.Id:
                    return roomItem.PhysicsItems.Find(x => x.Id == physicsItem.Id);
                case MobDTO mob when mob.Id == roomItem.Id:
                    return roomItem.Mobs.Find(x => x.Id == mob.Id);
            }
        }
        return null;
    }
    
    public static void RemoveItem(this WorldDTO worldDto, AItemDTO item)
    {
        if (item is WorldDTO world && world.Id == worldDto.Id) return;
        if (item is RoomDTO room)
        {
            var roomToRemove = worldDto.Rooms.Find(x => x.Id == room.Id);
            if (roomToRemove != null) worldDto.Rooms.Remove(roomToRemove);
            return;
        }
        if (item is FacadeDTO facade)
        {
            var facadeToRemove = worldDto.Facades.Find(x => x.Id == facade.Id);
            if (facadeToRemove != null) worldDto.Facades.Remove(facadeToRemove);
            return;
        }
        foreach (var roomItem in worldDto.Rooms)
        {
            switch (item)
            {
                case PhysicsItemDTO physicsItem:
                    var physicsItemToRemove = roomItem.PhysicsItems.Find(x => x.Id == physicsItem.Id);
                    if (physicsItemToRemove != null) roomItem.PhysicsItems.Remove(physicsItemToRemove);
                    break;
                case MobDTO mob:
                    var mobToRemove = roomItem.Mobs.Find(x => x.Id == mob.Id);
                    if (mobToRemove != null) roomItem.Mobs.Remove(mobToRemove);
                    break;
            }
        }
    }
}