using System;
using System.Diagnostics;
using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public class WorldMapper : IWorldMapper
    {
        private readonly WorldMapperContext mapperContext;
        private readonly WorldDTO worldDto;

        public WorldMapper(WorldDTO worldDto, WorldMapperContext mapperContext)
        {
            this.worldDto = worldDto;
            this.mapperContext = mapperContext;
        }

        public bool TryToGetWorld(out World? world)
        {
            try
            {
                world = mapperContext.WorldFactory.CreateWorld(worldDto, mapperContext);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                world = null;
            }
            return world != null;
        }
    }
}
