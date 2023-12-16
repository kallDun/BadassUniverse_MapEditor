using System;
using System.Windows;
using MapEditor.Extensions.Models;
using MapEditor.Models;
using MapEditor.Models.Game;
using MapEditor.Models.Server;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties;

namespace MapEditor.Services
{
    public class PreviewService : AService
    {
        private static LocalStorageService StorageService
            => ServicesManager.Instance.GetService<LocalStorageService>();
        private static PropertiesService PropertiesService
            => ServicesManager.Instance.GetService<PropertiesService>();
        
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
                case ItemType.PhysicsItem:
                    var physicsItem = previewItem as PhysicsItemDTO ?? throw new ArgumentException("Cannot cast item to PhysicsItemDTO.");
                    physicsItem.Color = ColorExtensions.GetRandomColor();
                    physicsItem.State = StoredPreviewState.Preview;
                    break;
                case ItemType.Mob:
                    var mob = previewItem as MobDTO ?? throw new ArgumentException("Cannot cast item to MobDTO.");
                    mob.Color = ColorExtensions.GetRandomColor();
                    mob.State = StoredPreviewState.Preview;
                    break;
            }
            StorageService.SetPreviewWorld(worldDTOPreview);
            PropertiesService.SetActiveItem(previewItem);
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
                    room.OnValueChanged?.Invoke("MapOffsetX");
                    room.OnValueChanged?.Invoke("MapOffsetY");
                    break;
                case ItemType.Building:
                    var facade = previewItem as FacadeDTO ?? throw new ArgumentException("Cannot cast item to FacadeDTO.");
                    facade.MapOffsetX = position.X;
                    facade.MapOffsetY = position.Y;
                    facade.OnValueChanged?.Invoke("MapOffsetX");
                    facade.OnValueChanged?.Invoke("MapOffsetY");
                    break;
            }
            StorageService.SetPreviewWorld(worldDTOPreview);
        }
        
        public void TryToMoveRoomItem(MapIndex position, Point mousePositionCellPercent, Room? room)
        {
            if (!(previewItem != null && previewItemType != null && worldDTOPreview != null)) return;
            if (previewItemType is not (ItemType.PhysicsItem or ItemType.Mob)) return;

            worldDTOPreview = (WorldDTO)StorageService.WorldDTO.Clone();
            
            if (room == null)
            {
                StorageService.SetPreviewWorld(worldDTOPreview);
                return;
            }
            var roomDto = worldDTOPreview.Rooms.Find(r => r.Id == room.Id);
            if (roomDto == null)
            {
                StorageService.SetPreviewWorld(worldDTOPreview);
                return;
            }

            const int itemCellSize = 100;
            MapIndex localPosition = position - room.LeftTopCorner;
            Point offset = new(
                localPosition.X * itemCellSize + mousePositionCellPercent.X * itemCellSize, 
                localPosition.Y * itemCellSize + mousePositionCellPercent.Y * itemCellSize);
            
            switch (previewItemType)
            {
                case ItemType.PhysicsItem:
                    var physicsItem = previewItem as PhysicsItemDTO ?? throw new ArgumentException("Cannot cast item to PhysicsItemDTO.");
                    physicsItem.Id = roomDto.GetNextPhysicsItemId();
                    physicsItem.RoomId = room.Id;
                    physicsItem.RoomOffsetX = (int)offset.X;
                    physicsItem.RoomOffsetY = (int)offset.Y;
                    physicsItem.OnValueChanged?.Invoke("Id");
                    physicsItem.OnValueChanged?.Invoke("RoomId");
                    physicsItem.OnValueChanged?.Invoke("RoomOffsetX");
                    physicsItem.OnValueChanged?.Invoke("RoomOffsetY");
                    roomDto.PhysicsItems.Add(physicsItem);
                    break;
                case ItemType.Mob:
                    var mob = previewItem as MobDTO ?? throw new ArgumentException("Cannot cast item to MobDTO.");
                    mob.Id = roomDto.GetNextMobId();
                    mob.RoomId = room.Id;
                    mob.RoomOffsetX = (int)offset.X;
                    mob.RoomOffsetY = (int)offset.Y;
                    mob.OnValueChanged?.Invoke("Id");
                    mob.OnValueChanged?.Invoke("RoomId");
                    mob.OnValueChanged?.Invoke("RoomOffsetX");
                    mob.OnValueChanged?.Invoke("RoomOffsetY");
                    roomDto.Mobs.Add(mob);
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
                var itemRef = previewItem;
                TryToCancel();
                PropertiesService.SetActiveItem(itemRef);
            }
        }
        
        public void TryToCancel()
        {
            if (!IsPreviewing) return;
            StorageService.SetPreviewWorld(null);
            previewItem = null;
            previewItemType = null;
            worldDTOPreview = null;
            PropertiesService.Reset();
        }
    }
}
