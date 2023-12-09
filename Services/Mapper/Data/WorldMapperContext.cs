using MapEditor.Services.Mapper.Factories;
using MapEditor.Services.Storage;

namespace MapEditor.Services.Mapper
{
    public class WorldMapperContext
    {
        public WorldMapperContext(string version, IGameStorage gameStorage, IListStorage listStorage, IWorldFactory worldFactory, ISubFactory[] subFactories)
        {
            Version = version;
            GameStorage = gameStorage;
            ListStorage = listStorage;
            WorldFactory = worldFactory;
            SubFactories = subFactories;
        }

        public string Version { get; private set; }

        public IGameStorage GameStorage { get; private set; }
        
        public IListStorage ListStorage { get; private set; }

        public IWorldFactory WorldFactory { get; private set; }

        public ISubFactory[] SubFactories { get; private set; }
    }
}
