using System;
using BadassUniverse_MapEditor.Extensions.Models;
using BadassUniverse_MapEditor.Models;
using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;
using BadassUniverse_MapEditor.Services.Manager;

namespace BadassUniverse_MapEditor.Services
{
    public class PreviewService : AService
    {
        private static LocalStorageService StorageService
            => ServicesManager.Instance.GetService<LocalStorageService>();
        
        private AItemDTO? previewItem;
        private ItemType? previewItemType;
        private WorldDTO? worldDTOPreview;

        public bool IsPreviewing => previewItem != null && previewItemType != null && worldDTOPreview != null;
        
        public void SetPreviewItem(AItemDTO item, ItemType type)
        {
            if (IsPreviewing)
            {
                TryToCancel();
            }
            
            previewItem = (AItemDTO)item.Clone();
            previewItemType = type;
            worldDTOPreview = (WorldDTO)StorageService.WorldDTO.Clone();
            switch (previewItemType)
            {
                case ItemType.Room:
                    var room = previewItem as RoomDTO ?? throw new ArgumentException("Cannot cast item to RoomDTO.");
                    room.Id = worldDTOPreview.GetNextRoomId();
                    room.MapId = worldDTOPreview.Id;
                    room.Color = ColorExtensions.GetRandomColor();
                    room.Floor = StorageService.CurrentFloor;
                    room.State = StoredPreviewState.Preview;
                    worldDTOPreview.Rooms.Add(room);
                    break;
                case ItemType.Building:
                    var facade = previewItem as FacadeDTO ?? throw new ArgumentException("Cannot cast item to FacadeDTO.");
                    facade.Id = worldDTOPreview.GetNextFacadeId();
                    facade.MapId = worldDTOPreview.Id;
                    facade.Color = ColorExtensions.GetRandomColor();
                    facade.Floor = StorageService.CurrentFloor;
                    facade.State = StoredPreviewState.Preview;
                    worldDTOPreview.Facades.Add(facade);
                    break;
            }
            StorageService.SetPreviewWorld(worldDTOPreview);
        }

        public void TryToMoveRoomOrFacade(MapIndex position)
        {
            if (!(previewItem != null && previewItemType != null && worldDTOPreview != null)) return;
            if (previewItemType is not (ItemType.Room or ItemType.Building)) return;
            
            switch (previewItemType)
            {
                case ItemType.Room:
                    var room = previewItem as RoomDTO ?? throw new ArgumentException("Cannot cast item to RoomDTO.");
                    room.MapOffsetX = position.X;
                    room.MapOffsetY = position.Y;
                    room.OnValuesChanged?.Invoke();
                    break;
                case ItemType.Building:
                    var facade = previewItem as FacadeDTO ?? throw new ArgumentException("Cannot cast item to FacadeDTO.");
                    facade.MapOffsetX = position.X;
                    facade.MapOffsetY = position.Y;
                    facade.OnValuesChanged?.Invoke();
                    break;
            }
            StorageService.SetPreviewWorld(worldDTOPreview);
        }

        public void TryToSave()
        {
            if (!(previewItem != null && previewItemType != null && worldDTOPreview != null)) return;
            if (StorageService.IsPreviewWorldValid)
            {
                previewItem.State = StoredPreviewState.Stored;
                StorageService.SetWorld(worldDTOPreview);
                TryToCancel();
            }
        }
        
        public void TryToCancel()
        {
            if (!IsPreviewing) return;
            StorageService.SetPreviewWorld(null);
            previewItem = null;
            previewItemType = null;
            worldDTOPreview = null;
        }
    }
}
