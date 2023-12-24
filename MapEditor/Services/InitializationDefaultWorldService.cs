using System;
using System.Windows;
using System.Windows.Media;
using MapEditor.Models.Game;
using MapEditor.Models.Server;
using MapEditor.Services.Manager;

namespace MapEditor.Services
{
    public class InitializationDefaultWorldService : AService
    {
        private static LocalStorageService StorageService
            => ServicesManager.Instance.GetService<LocalStorageService>();

        public override void Initialize()
        {
            base.Initialize();

            WorldDTO worldDTO = new()
            {
                Id = 0,
                Name = "Default World",
                XLenght = 50,
                YLenght = 50,
                PlayerSpawnRoomId = 0,
                Version = App.Version
            };
            worldDTO.Rooms.Add(new RoomDTO
            {
                Id = 0,
                MapId = worldDTO.Id,
                InGameRoomId = 0,
                Name = StorageService.GetGameStorage().GetRoomData(0)?.Name ?? "Room",
                Color = Color.FromRgb(0, 120, 120),
                MapOffsetX = 25,
                MapOffsetY = 25,
                Rotation = MapDirection.Up
            });
            worldDTO.Rooms.Add(new RoomDTO
            {
                Id = 1,
                MapId = worldDTO.Id,
                InGameRoomId = 0,
                Name = StorageService.GetGameStorage().GetRoomData(0)?.Name ?? "Room",
                Color = Color.FromRgb(100, 100, 150),
                MapOffsetX = 14,
                MapOffsetY = 24,
                Rotation = MapDirection.Down
            });

            StorageService.SetWorld(worldDTO);
        }
    }
}
