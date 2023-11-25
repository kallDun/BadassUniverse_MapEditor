using BadassUniverse_MapEditor.Services.Mapper.Factories;
using BadassUniverse_MapEditor.Services.Storage;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public class WorldMapperContext
    {
        public WorldMapperContext(IGameStorage gameStorage, IWorldFactory worldFactory, ISubFactory[] subFactories)
        {
            GameStorage = gameStorage;
            WorldFactory = worldFactory;
            SubFactories = subFactories;
        }

        public IGameStorage GameStorage { get; private set; }

        public IWorldFactory WorldFactory { get; private set; }

        public ISubFactory[] SubFactories { get; private set; }
    }
}
