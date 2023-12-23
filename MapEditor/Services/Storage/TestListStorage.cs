using System;
using System.Collections.Generic;
using MapEditor.Models.Game.Data;
using MapEditor.Models.Server;
using Newtonsoft.Json;

namespace MapEditor.Services.Storage;

public class TestListStorage : IListStorage
{
    public IEnumerable<RoomDTO> GetRooms(IGameStorage storage)
    {
        List <RoomDTO> rooms = new()
        {
            new RoomDTO
            {
                InGameRoomId = 0,
                Name = storage.GetRoomData(0)?.Name ?? "Room"
            },
            new RoomDTO
            {
                InGameRoomId = 1,
                Name = storage.GetRoomData(1)?.Name ?? "Room",
                Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 15, Length = 10 }),
                DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
            },
        };
        return rooms;
    }

    public IEnumerable<FacadeDTO> GetFacades(IGameStorage storage)
    {
        List<FacadeDTO> facades = new()
        {
            new FacadeDTO
            {
                InGameFacadeId = 0,
                Name = storage.GetFacadeData(0)?.Name ?? "Facade"
            }
        };
        return facades;
    }

    public IEnumerable<PhysicsItemDTO> GetPhysicsItems(IGameStorage storage)
    {
        List<PhysicsItemDTO> physicsItems = new()
        {
            new PhysicsItemDTO
            {
                InGamePhysicsItemId = 0,
                Name = storage.GetPhysicsItemData(0)?.Name ?? "Physics Item"
            }
        };
        return physicsItems;
    }

    public IEnumerable<MobDTO> GetMobs(IGameStorage storage)
    {
        List<MobDTO> mobs = new()
        {
            new MobDTO
            {
                InGameMobId = 0,
                Name = storage.GetMobData(0)?.Name ?? "Mob"
            }
        };
        return mobs;
    }
}
