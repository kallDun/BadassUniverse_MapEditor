using MapEditor.Models.Game;

namespace MapEditor.Services.Mapper
{
    public interface IWorldMapper
    {
        bool TryToGetWorld(out World? world);
    }
}
