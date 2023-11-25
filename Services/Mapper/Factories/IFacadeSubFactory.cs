﻿using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Mapper.Factories
{
    public interface IFacadeSubFactory : ISubFactory
    {
        Facade CreateFacade(FacadeDTO facade);
    }
}
