using System;
using System.Collections.Generic;
using System.Windows.Media;
using BadassUniverse_MapEditor.Models;
using BadassUniverse_MapEditor.Models.Game;
using BadassUniverse_MapEditor.Models.Server;

namespace BadassUniverse_MapEditor.Services.Storage;

public class TestListStorage : IListStorage
{
    public IEnumerable<RoomDTO> GetRooms(IGameStorage storage)
    {
        List <RoomDTO> rooms = new()
        {
            new RoomDTO
            {
                Id = 0,
                MapId = 0,
                InGameRoomId = 0,
                Name = storage.GetRoomData(0)?.Name 
                       ?? throw new ArgumentException($"Room with id {0} not found."),
                Color = Color.FromRgb(0, 0, 0),
                MapOffsetX = 0,
                MapOffsetY = 0,
                Rotation = MapDirection.Up,
                Floor = 0,
                PhysicsItems = new List<PhysicsItemDTO>(),
                Mobs = new List<MobDTO>(),
                State = StoredPreviewState.Preview
            }
        };
        return rooms;
    }
}
