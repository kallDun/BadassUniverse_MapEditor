using System;
using MapEditor.Models.Game;
using MapEditor.Models.Server;
using MapEditor.Services.Storage;
using MapEditor.Services.Storage.Data;

namespace MapEditor.Services.Mapper.Factories;

public class BasicFacadeSubFactory : IFacadeSubFactory
{
    public Facade CreateFacade(FacadeDTO facade, IGameStorage gameStorage)
    {
        FacadeStorageData data = gameStorage.GetFacadeData(facade.InGameFacadeId)
                                 ?? throw new ArgumentException($"Facade with id {facade.InGameFacadeId} does not exist.");

        return new Facade(facade.Id, data.Name, facade.Floor, facade.Color, facade.Rotation, facade.State, data.Params);
    }
}