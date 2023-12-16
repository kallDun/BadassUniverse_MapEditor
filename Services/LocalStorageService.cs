using System;
using MapEditor.Models.Game;
using MapEditor.Models.Server;
using MapEditor.Services.Manager;
using MapEditor.Services.Mapper;
using MapEditor.Services.Storage;

namespace MapEditor.Services
{
    public class LocalStorageService : AService
    {
        private World? world;
        private WorldDTO? worldDTO;
        private WorldDTO? worldDTOPreview;
        private WorldMapperContext? worldMapperContext;
        private IWorldMapper? worldMapper;

        public Action? OnWorldChanged { get; set; }
        public Action? OnMapperContextChanged { get; set; }

        public World World => world ?? throw new Exception("World is null.");
        public WorldDTO WorldDTO => worldDTO ?? throw new Exception("WorldDTO is null.");
        public bool IsPreviewWorldValid { get; private set; } = false;
        public int CurrentFloor { get; private set; } = 0;

        public override void Initialize()
        {
            base.Initialize();
            worldMapperContext ??= WorldMapperContextFactory.GetDefaultContext();
            OnMapperContextChanged?.Invoke();
        }
        
        public override void Destroy() 
            => throw new Exception("LocalStorageService cannot be destroyed.");
        
        public IGameStorage GetGameStorage() => worldMapperContext?.GameStorage 
                                                ?? throw new Exception("World Context is null. Initialize Local Storage first!");
        public IListStorage GetListStorage() => worldMapperContext?.ListStorage 
                                                ?? throw new Exception("World Context is null. Initialize Local Storage first!");
        
        public void SetPreviewWorld(WorldDTO? inWorldDto)
        {
            if (worldDTO is null) throw new Exception("WorldDTO is null.");
            if (inWorldDto != null)
            {
                worldDTOPreview = inWorldDto;
                IsPreviewWorldValid = TryToSetWorld(worldDTOPreview);
                if (!IsPreviewWorldValid)
                {
                    TryToSetWorld(worldDTO);
                }
            }
            else
            {
                worldDTOPreview = null;
                IsPreviewWorldValid = false;
                TryToSetWorld(worldDTO);
            }
        }
        
        public void SetWorld(WorldDTO inWorldDto)
        {
            if (TryToSetWorld(inWorldDto))
            {
                worldDTO = inWorldDto;
            }
            else throw new ArgumentException("Cannot create world from mapDTO.");
        }
        
        public void UpdateWorld()
        {
            if (worldDTO is null) throw new Exception("WorldDTO is null.");
            if (worldDTOPreview == null || !TryToSetWorld(worldDTOPreview))
            {
                SetWorld(worldDTO);
            }
        }
        
        private bool TryToSetWorld(WorldDTO worldDto)
        {
            if (worldMapperContext == null || worldMapperContext.Version != worldDto.Version)
            {
                worldMapperContext = WorldMapperContextFactory.GetContext(worldDto.Version);
                OnMapperContextChanged?.Invoke();
            }
            worldMapper = new WorldMapper(worldDto, worldMapperContext);
            if (!worldMapper.TryToGetWorld(out World? worldData)) return false;
            world = worldData;
            OnWorldChanged?.Invoke();
            return true;
        }
    }
}
