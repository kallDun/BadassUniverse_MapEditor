using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Manager;
using BadassUniverse_MapEditor.Services.Mapper;
using BadassUniverse_MapEditor.Services.Storage;
using System;

namespace BadassUniverse_MapEditor.Services
{
    public class LocalStorageService : AService
    {
        private World? world;
        private WorldDTO? worldDTO;
        private WorldMapperContext? worldMapperContext;
        private IWorldMapper? worldMapper;

        public Action? OnWorldChanged { get; set; }
        public World World => world ?? throw new Exception("World is null.");

        public override void Initialize()
        {
            base.Initialize();
            worldMapperContext ??= WorldMapperContextFactory.GetDefaultContext();
        }

        public void SetWorld(WorldDTO worldDTO)
        {
            if (worldMapperContext == null || worldMapperContext.Version != worldDTO.Version)
            {
                worldMapperContext = WorldMapperContextFactory.GetContext(worldDTO.Version);
            }
            worldMapper = new WorldMapper(worldDTO, worldMapperContext);
            if (worldMapper.TryToGetWorld(out World? world))
            {
                this.worldDTO = worldDTO;
                this.world = world;
                OnWorldChanged?.Invoke();
            }
            else throw new ArgumentException("Cannot create world from mapDTO.");
        }

        public IGameStorage GetGameStorage() => worldMapperContext?.GameStorage 
            ?? throw new Exception("World Context is null. Initialize Local Storage first!");

        public override void Destroy() 
            => throw new Exception("LocalStorageService cannot be destroyed.");
    }
}
