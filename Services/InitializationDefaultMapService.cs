﻿using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Manager;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BadassUniverse_MapEditor.Services
{
    public class InitializationDefaultMapService : AService
    {
        private static LocalStorageService StorageService
            => ServicesManager.Instance.GetService<LocalStorageService>();

        public override void Initialize()
        {
            base.Initialize();

            MapDTO mapDTO = new()
            {
                Id = 0,
                Name = "Default Map",
                XLenght = 50,
                YLenght = 50,
                PlayerSpawnRoomId = 0,
                Rooms = new List<RoomDTO>(),
                Facades = new List<FacadeDTO>(),
                Version = (Application.Current as App 
                    ?? throw new Exception("Application cannot be null.")).Version
            };
            mapDTO.Rooms.Add(new RoomDTO
            {
                Id = 0,
                MapId = 0,
                InGameRoomId = 0,
                Name = StorageService.GetRoomName(gameRoomId: 0),
                Color = Color.FromRgb(0, 120, 120),
                MapOffsetX = 25,
                MapOffsetY = 25,
                Rotation = MapDirection.Up,
                Floor = 0,
                PhysicsItems = new List<PhysicsItemDTO>(),
                Mobs = new List<MobDTO>()
            });
            mapDTO.Rooms.Add(new RoomDTO
            {
                Id = 1,
                MapId = 0,
                InGameRoomId = 0,
                Name = StorageService.GetRoomName(gameRoomId: 0),
                Color = Color.FromRgb(0, 0, 120),
                MapOffsetX = 14,
                MapOffsetY = 24,
                Rotation = MapDirection.Down,
                Floor = 0,
                PhysicsItems = new List<PhysicsItemDTO>(),
                Mobs = new List<MobDTO>()
            });

            StorageService.SetMap(mapDTO);
        }
    }
}
