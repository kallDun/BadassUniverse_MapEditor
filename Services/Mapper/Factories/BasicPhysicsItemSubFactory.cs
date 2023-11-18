using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using System;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public class BasicPhysicsItemSubFactory : IPhysicsItemSubFactory
    {
        Type ISubFactory.SubType => typeof(PhysicsItem);

        PhysicsItem IPhysicsItemSubFactory.CreatePhysicsItem(PhysicsItemDTO item)
        {
            throw new NotImplementedException();
        }
    }
}