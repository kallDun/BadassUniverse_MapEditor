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
    [Test]
    public void SetPreviewRoomOkAndThenIntersectWithAnotherRoomTest()
    {
        ServicesManager.Instance.ResetAllServices();
        var localStorageService = ServicesManager.Instance.GetService<LocalStorageService>();
        var previewService = ServicesManager.Instance.GetService<PreviewService>();
        
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
            localStorageService.SetWorld(worldDTO);
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
        
        previewService.SetPreviewItem(previewRoom, ItemType.Room);
        Assert.That(previewService.IsPreviewing, Is.True);
        
        previewService.TryToMoveRoomOrFacade(new MapIndex(8, 8));
        Assert.That(localStorageService.IsPreviewWorldValid, Is.True);
        
        previewService.TryToMoveRoomOrFacade(new MapIndex(0, 0));
        Assert.That(localStorageService.IsPreviewWorldValid, Is.False);
        Assert.That(previewService.TryToSave(), Is.False);
        
        previewService.TryToMoveRoomOrFacade(new MapIndex(7, 7));
        Assert.That(localStorageService.IsPreviewWorldValid, Is.True);
        Assert.That(previewService.TryToSave(), Is.True);
    }
    
}