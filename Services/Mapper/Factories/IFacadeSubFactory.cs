using MapEditor.Models.Game;
using MapEditor.Models.Server;
using MapEditor.Services.Storage;

namespace MapEditor.Services.Mapper.Factories
{
    public interface IFacadeSubFactory : ISubFactory
    {
        Facade CreateFacade(FacadeDTO facade, IGameStorage gameStorage);
    }
}
