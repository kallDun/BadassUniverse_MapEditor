﻿using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Manager;
using BadassUniverse_MapEditor.Services.Mapper;
using System;

namespace BadassUniverse_MapEditor.Services
{
    public class LocalStorageService : AService
    {
        private World? world;
        private IWorldMapper? worldMapper;

        public Action? OnWorldChanged { get; set; }
        public World World => world ?? throw new Exception("World is null.");

        public void SetMap(MapDTO mapDTO)
        {
            worldMapper = new WorldMapper(mapDTO);
            if (worldMapper.TryToGetWorld(out World world))
            {
                this.world = world;
                OnWorldChanged?.Invoke();
            }
            else throw new ArgumentException("Cannot create world from mapDTO");
        }

        public string GetRoomName(int roomId)
        {
            WorldMapperContext context = worldMapper?.MapperContext
                ?? WorldMapperContextGenerator.GenerateDefaultContext();
            return context.GameStorage.GetRoomData(roomId)?.Name
                ?? throw new ArgumentException($"Room with id {roomId} not found.");
        }

        public override void Destroy() => throw new Exception("LocalStorageService cannot be destroyed");
    }
}
