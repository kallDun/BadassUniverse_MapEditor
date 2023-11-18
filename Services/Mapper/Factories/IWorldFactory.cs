using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public interface IWorldFactory
    {
        /// <returns>World object if convert was successful, otherwise - NULL</returns>
        public abstract World CreateWorld(MapDTO map, ISubFactory[] factories);
    }
}
