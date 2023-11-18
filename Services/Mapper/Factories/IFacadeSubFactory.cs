using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public interface IFacadeSubFactory : ISubFactory
    {
        protected abstract Facade CreateFacade(FacadeDTO facade);
    }
}
