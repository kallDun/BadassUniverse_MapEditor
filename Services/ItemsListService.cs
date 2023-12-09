using System;
using System.Collections.Generic;
using System.Linq;
using BadassUniverse_MapEditor.Models;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Manager;
using BadassUniverse_MapEditor.Services.Storage;

namespace BadassUniverse_MapEditor.Services
{
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
            List<(AItemDTO Item, ItemType Type)> list = new();

            var rooms = listStorage.GetRooms(gameStorage)
                .Select(item => (Item: item as AItemDTO, Type: ItemType.Room));
            list.AddRange(rooms);
            
            return list;
        }
    }
}
