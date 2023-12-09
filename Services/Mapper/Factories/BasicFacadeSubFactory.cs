using System;
using MapEditor.Models.Game;
using MapEditor.Models.Server;
using MapEditor.Services.Storage;

namespace MapEditor.Services.Mapper.Factories
{
    public class BasicFacadeSubFactory : IFacadeSubFactory
    {
        Facade IFacadeSubFactory.CreateFacade(FacadeDTO facade, IGameStorage gameStorage)
        {
            throw new NotImplementedException();
        }
    }
}