using MapEditor.Models.Game;
using MapEditor.Models.Server;

namespace MapEditor.Services.Mapper.Factories;

public class BasicPhysicsItemSubFactory : IPhysicsItemSubFactory
{
    public PhysicsItem CreatePhysicsItem(PhysicsItemDTO itemDto, Room roomOwner)
    {
        return new PhysicsItem(itemDto.Id, itemDto.Name, 
            new MapIndex(itemDto.RoomOffsetY, itemDto.RoomOffsetX), 
            itemDto.Color,$"item_{(int)itemDto.Icon}", itemDto.State, roomOwner);
    }
}