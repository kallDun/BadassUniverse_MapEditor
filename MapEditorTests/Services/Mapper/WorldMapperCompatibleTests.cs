using MapEditor;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Data;
using MapEditor.Models.Server;
using MapEditor.Services.Mapper;
using Newtonsoft.Json;

namespace MapEditorTests.Services.Mapper;

public class WorldMapperCompatibleTests
{
    private static void CompatibleWorldTest(WorldDTO worldDto)
    {
        var worldMapperContext = WorldMapperContextFactory.GetContext(worldDto.Version);
        var worldMapper = new WorldMapper(worldDto, worldMapperContext);
        bool result = worldMapper.TryToGetWorld(out World? world);
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void NormalSizeWorldTest()
    {
        CompatibleWorldTest(new WorldDTO()
        {
            Id = 0, Name = "Test",
            XLenght = 10, YLenght = 10,
            PlayerSpawnRoomId = 0, Version = App.Version,
            Facades = { },
            Rooms =
            {
                new RoomDTO
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 3, MapOffsetY = 3,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 5, Length = 5 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
            }
        });
    }

    [Test]
    public void RoomsIntersectionTest()
    {
        CompatibleWorldTest(new WorldDTO()
        {
            Id = 0, Name = "Test",
            XLenght = 15, YLenght = 15,
            PlayerSpawnRoomId = 0, Version = App.Version,
            Facades = { },
            Rooms =
            {
                new RoomDTO
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 0, MapOffsetY = 0,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 5, Length = 5 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
                new RoomDTO
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 6, MapOffsetY = 6,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 5, Length = 5 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
            }
        });
    }
    
    [Test]
    public void RoomsIntersectionTest2()
    {
        CompatibleWorldTest(new WorldDTO()
        {
            Id = 0, Name = "Test",
            XLenght = 30, YLenght = 30,
            PlayerSpawnRoomId = 0, Version = App.Version,
            Facades = { },
            Rooms =
            {
                new RoomDTO
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 0, MapOffsetY = 0,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 10, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
                new RoomDTO
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 0, MapOffsetY = 11,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 10, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
            }
        });
    }
    
    [Test]
    public void FacadesIntersectionTest()
    {
        CompatibleWorldTest(new WorldDTO()
        {
            Id = 0, Name = "Test",
            XLenght = 10, YLenght = 10,
            PlayerSpawnRoomId = 0, Version = App.Version,
            Rooms = { },
            Facades =
            {
                new FacadeDTO
                {
                    InGameFacadeId = 0, // facade with id=0 size is 3x6
                    Name = "TestFacade",
                    MapOffsetX = 0, MapOffsetY = 0
                },
                new FacadeDTO
                {
                    InGameFacadeId = 0,
                    Name = "TestFacade",
                    MapOffsetX = 0, MapOffsetY = 0
                },
            }
        });
    }
}