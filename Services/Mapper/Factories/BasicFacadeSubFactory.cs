using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using System;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public class BasicFacadeSubFactory : IFacadeSubFactory
    {
        Type ISubFactory.SubType => typeof(Facade);

        Facade IFacadeSubFactory.CreateFacade(FacadeDTO facade)
        {
            throw new NotImplementedException();
        }
    }
}