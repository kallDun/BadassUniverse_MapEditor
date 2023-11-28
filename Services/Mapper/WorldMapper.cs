using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public class WorldMapper : IWorldMapper
    {
        private WorldMapperContext mapperContext;
        private WorldDTO worldDto;

        public WorldMapper(WorldDTO worldDto, WorldMapperContext mapperContext)
        {
            this.worldDto = worldDto;
            this.mapperContext = mapperContext;
        }

        public bool TryToGetWorld(out World world)
        {
            world = mapperContext.WorldFactory.CreateWorld(worldDto, mapperContext);
            return world != null;
        }
    }
}
