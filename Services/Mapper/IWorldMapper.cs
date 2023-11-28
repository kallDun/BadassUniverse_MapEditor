using BadassUniverse_MapEditor.Models.Game;

namespace BadassUniverse_MapEditor.Services.Mapper
{
    public interface IWorldMapper
    {
        bool TryToGetWorld(out World world);
    }
}
