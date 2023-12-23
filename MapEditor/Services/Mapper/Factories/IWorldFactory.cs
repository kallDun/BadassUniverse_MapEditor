using MapEditor.Models.Game;
using MapEditor.Models.Server;

namespace MapEditor.Services.Mapper.Factories
{
    public interface IWorldFactory
    {
        /// <returns>World object if convert was successful, otherwise - NULL</returns>
        World CreateWorld(WorldDTO worldDto, WorldMapperContext mapperContext);
    }
}
