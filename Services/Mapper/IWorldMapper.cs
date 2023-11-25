using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public interface IWorldMapper
    {
        WorldMapperContext MapperContext { get; }

        MapDTO MapDTO { get; }

        bool TryToGetWorld(out World world);
    }
}
