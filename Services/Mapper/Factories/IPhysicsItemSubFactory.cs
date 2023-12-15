using MapEditor.Models.Game;
using MapEditor.Models.Server;

namespace MapEditor.Services.Mapper.Factories
{
    public interface IPhysicsItemSubFactory : ISubFactory
    {
        PhysicsItem CreatePhysicsItem(PhysicsItemDTO physicsItem, Room roomOwner);
    }
}
