using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Mapper.Factories;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public class WorldMapper : IWorldMapper
    {
        public bool TryToGetWorld(MapDTO mapDTO, out World world)
        {
            IWorldFactory factory = new BasicWorldFactory();
            ISubFactory[] subFactories = new ISubFactory[]
            {
                new BasicRoomSubFactory(), new BasicFacadeSubFactory(), new BasicPhysicsItemSubFactory(), new BasicMobSubFactory()
            };
            world = factory.CreateWorld(mapDTO, subFactories);
            return world != null;
        }
    }
}
