using MapEditor;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Data;
using MapEditor.Models.Server;
using MapEditor.Services.Mapper;
using Newtonsoft.Json;

namespace MapEditorTests.Services.Mapper;

public class WorldMapperIncompatibleTests
{
    private static void IncompatibleWorldTest(WorldDTO worldDto)
    {
        var worldMapperContext = WorldMapperContextFactory.GetContext(worldDto.Version);
        var worldMapper = new WorldMapper(worldDto, worldMapperContext);
        bool result = worldMapper.TryToGetWorld(out World? world);
        Assert.That(result, Is.False);
    }
    
    [Test]
    public void SmallWorldTest()
    {
        IncompatibleWorldTest(new WorldDTO()
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
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 15, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
            }
        });
    }
    
    [Test]
    public void RoomIntersectionTest()
    {
        IncompatibleWorldTest(new WorldDTO()
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
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 15, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
                new RoomDTO
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 5, MapOffsetY = 5,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 10, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
            }
        });
    }
    
    [Test]
    public void RoomIntersectionTest2()
    {
        IncompatibleWorldTest(new WorldDTO()
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
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 15, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
                new RoomDTO
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 5, MapOffsetY = 5,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 10, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
                new RoomDTO
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 10, MapOffsetY = 10,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 10, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                },
            }
        });
    }

    [Test]
    public void RoomOutsideTheWorldTest()
    {
        IncompatibleWorldTest(new WorldDTO()
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
                    MapOffsetX = 19, MapOffsetY = 19,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 10, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                }
            }
        });
    }
    
    [Test]
    public void BelowZeroPositionValuesTest()
    {
        IncompatibleWorldTest(new WorldDTO()
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
                    MapOffsetX = -1, MapOffsetY = -1,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 10, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                }
            }
        });
    }

    [Test]
    public void SameDoorIntersectionStreetRoomTest()
    {
        IncompatibleWorldTest(new WorldDTO()
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
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 15, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(new StreetRoomDoorParameters[]
                    {
                        new() { Direction = MapDirection.Down, Length = 1, StartIndex = 1 },
                        new() { Direction = MapDirection.Down, Length = 1, StartIndex = 1 }
                    })
                }
            }
        });
    }
    
    [Test]
    public void SameDoorIntersectionStreetRoomTest2()
    {
        IncompatibleWorldTest(new WorldDTO()
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
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() { Width = 15, Length = 10 }),
                    DoorParams = JsonConvert.SerializeObject(new StreetRoomDoorParameters[]
                    {
                        new() { Direction = MapDirection.Right, Length = 4, StartIndex = 1 },
                        new() { Direction = MapDirection.Right, Length = 2, StartIndex = 4 }
                    })
                }
            }
        });
    }
    
}