using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Mapper.Factories;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public interface IWorldMapper
    {
        bool TryToGetWorld(MapDTO mapDTO, out World world);
    }
}
