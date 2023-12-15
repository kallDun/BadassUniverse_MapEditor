using System.Collections.Generic;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Data;
using MapEditor.Models.Game.Rooms;
using MapEditor.Services.Storage.Data;

namespace MapEditor.Services.Storage;

public class TestGameStorage : IGameStorage
{
    public IEnumerable<RoomStorageData> GetRoomsData()
    {
        return new List<RoomStorageData>
        {
            new()
            {
                Id = 0,
                Name = "Square Room",
                Params = AnyFormBuildingParameters.FromSquareParameters(new SquareBuildingParameters
                {
                    Width = 10,
                    Length = 10,
                }),
                DoorParams = new RoomDoorParameters[]
                {
                    new()
                    {
                        Id = 0,
                        Position = new MapIndex(5, 0),
                        FloorDisplacement = 0
                    }
                },
                RoomType = typeof(AnyFormRoom)
            },
            new()
            {
                Id = 1,
                Name = "Street Room",
                RoomType = typeof(StreetRoom)
            }
        };
    }

    public IEnumerable<FacadeStorageData> GetFacadesData()
    {
        return new List<FacadeStorageData>
        {
            new()
            {
                Id = 0,
                Name = "Little Building",
                Params = AnyFormBuildingParameters.FromSquareParameters(new SquareBuildingParameters
                {
                    Width = 3,
                    Length = 6,
                })
            },
        };
    }

    public IEnumerable<PhysicsItemStorageData> GetPhysicsItemsData()
    {
        return new List<PhysicsItemStorageData>()
        {
            new()
            {
                Id = 0,
                Name = "Chest",
            }
        };
    }

    public IEnumerable<MobStorageData> GetMobsData()
    {
        return new List<MobStorageData>()
        {
            new()
            {
                Id = 0,
                Name = "Goblin",
            }
        };
    }
}