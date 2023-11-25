using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public class WorldMapper : IWorldMapper
    {
        private WorldMapperContext mapperContext;
        private MapDTO mapDTO;

        public WorldMapperContext MapperContext => mapperContext;

        public MapDTO MapDTO => mapDTO;

        public WorldMapper(MapDTO mapDTO)
        {
            this.mapDTO = mapDTO;
            mapperContext = WorldMapperContextFactory.GetContext(mapDTO.Version);
        }

        public bool TryToGetWorld(out World world)
        {
            world = mapperContext.WorldFactory.CreateWorld(mapDTO, mapperContext);
            return world != null;
        }
    }
}
