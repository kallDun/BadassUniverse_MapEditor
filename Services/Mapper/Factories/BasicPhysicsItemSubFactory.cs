using System;
using MapEditor.Models.Game;
using MapEditor.Models.Server;

namespace MapEditor.Services.Mapper.Factories
{
    public class BasicPhysicsItemSubFactory : IPhysicsItemSubFactory
    {
        PhysicsItem IPhysicsItemSubFactory.CreatePhysicsItem(PhysicsItemDTO item)
        {
            throw new NotImplementedException();
        }
    }
}