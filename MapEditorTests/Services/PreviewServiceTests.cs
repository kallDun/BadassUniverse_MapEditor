using MapEditor;
using MapEditor.Models;
using MapEditor.Models.Game;
using MapEditor.Models.Game.Data;
using MapEditor.Models.Server;
using MapEditor.Services;
using MapEditor.Services.Manager;
using Newtonsoft.Json;

namespace MapEditorTests.Services;

public class PreviewServiceTests
{
    private static LocalStorageService StorageService => ServicesManager.Instance.GetService<LocalStorageService>();
    private static PreviewService PreviewService => ServicesManager.Instance.GetService<PreviewService>();
    
    [SetUp]
    public void Setup()
    {
        ServicesManager.Instance.ResetAllServices();
    }
    
    [Test]
    public void SetPreviewRoomOkAndThenIntersectWithAnotherRoomTest()
    {
        WorldDTO worldDTO = new()
        {
            Id = 0,
            Name = "Default World",
            XLenght = 20,
            YLenght = 20,
            PlayerSpawnRoomId = 0,
            Version = App.Version,
            Rooms =
            {
                new RoomDTO()
                {
                    InGameRoomId = 1,
                    Name = "TestRoom",
                    MapOffsetX = 0, MapOffsetY = 0,
                    Params = JsonConvert.SerializeObject(new StreetRoomParameters() {Width = 5, Length = 5}),
                    DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
                }
            }
        };
        try
        {
            StorageService.SetWorld(worldDTO);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
        
        RoomDTO previewRoom = new()
        {
            InGameRoomId = 1,
            Name = "PreviewRoom",
            Params = JsonConvert.SerializeObject(new StreetRoomParameters() {Width = 5, Length = 5}),
            DoorParams = JsonConvert.SerializeObject(Array.Empty<StreetRoomDoorParameters>())
        };
        
        PreviewService.SetPreviewItem(previewRoom, ItemType.Room);
        Assert.That(PreviewService.IsPreviewing, Is.True);
        
        PreviewService.TryToMoveRoomOrFacade(new MapIndex(8, 8));
        Assert.That(StorageService.IsPreviewWorldValid, Is.True);
        
        PreviewService.TryToMoveRoomOrFacade(new MapIndex(0, 0));
        Assert.That(StorageService.IsPreviewWorldValid, Is.False);
        Assert.That(PreviewService.TryToSave(), Is.False);
        
        PreviewService.TryToMoveRoomOrFacade(new MapIndex(7, 7));
        Assert.That(StorageService.IsPreviewWorldValid, Is.True);
        Assert.That(PreviewService.TryToSave(), Is.True);
    }
    
}