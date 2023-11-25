using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Storage;
using System;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public class BasicFacadeSubFactory : IFacadeSubFactory
    {
        Facade IFacadeSubFactory.CreateFacade(FacadeDTO facade, IGameStorage gameStorage)
        {
            throw new NotImplementedException();
        }
    }
}