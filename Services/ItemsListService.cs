using BadassUniverse_MapEditor.Services.Manager;
using BadassUniverse_MapEditor.Services.Storage;
using BadassUniverse_MapEditor.Services.Storage.Data;
using System;
using System.Collections.Generic;
using System.Linq;

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
            StorageService.OnWorldChanged += OnWorldChanged;
        }

        public override void Destroy()
        {
            base.Destroy();
            StorageService.OnWorldChanged -= OnWorldChanged;
        }

        private void OnWorldChanged()
        {
            OnItemsListChanged?.Invoke();
        }

        public List<ItemData> LoadItems()
        {
            IGameStorage gameStorage = StorageService.GetGameStorage();
            List<RoomStorageData> rooms = gameStorage.GetRoomsData();
            List<ItemData> list = rooms.Select(x => new ItemData
            {
                Name = x.Name,
                Type = ItemType.Room
            }).ToList();
            return list;
        }
    }

    public class ItemData
    {
        public required string Name { get; set; }
        public required ItemType Type { get; set; }
    }

    public enum ItemType
    {
        Room,
        Building,
        Item,
        Mob
    }
}
