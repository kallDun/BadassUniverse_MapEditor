using System;
using System.Collections.Generic;
using System.Linq;
using MapEditor.Models;
using MapEditor.Models.Server;
using MapEditor.Services.Manager;
using MapEditor.Services.Storage;

namespace MapEditor.Services;

public class ItemsListService : AService
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();

    public Action? OnItemsListChanged { get; set; }

    public override void Initialize()
    {
        base.Initialize();
        StorageService.OnMapperContextChanged += MapperContextChanged;
    }

    public override void Destroy()
    {
        base.Destroy();
        StorageService.OnMapperContextChanged -= MapperContextChanged;
    }

    private void MapperContextChanged() => OnItemsListChanged?.Invoke();

    public List<(AItemDTO Item, ItemType Type)> LoadItems()
    {
        IGameStorage gameStorage = StorageService.GetGameStorage();
        IListStorage listStorage = StorageService.GetListStorage();
        List<(AItemDTO Item, ItemType Type)> list = [];

        var rooms = listStorage.GetRooms(gameStorage)
            .Select(item => (Item: item as AItemDTO, Type: ItemType.Room));
        list.AddRange(rooms);
            
        var facades = listStorage.GetFacades(gameStorage)
            .Select(item => (Item: item as AItemDTO, Type: ItemType.Building));
        list.AddRange(facades);
            
        var physicsItems = listStorage.GetPhysicsItems(gameStorage)
            .Select(item => (Item: item as AItemDTO, Type: ItemType.PhysicsItem));
        list.AddRange(physicsItems);
            
        var mobs = listStorage.GetMobs(gameStorage)
            .Select(item => (Item: item as AItemDTO, Type: ItemType.Mob));
        list.AddRange(mobs);
            
        return list;
    }
}