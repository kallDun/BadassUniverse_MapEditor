namespace MapEditorTests.Services;

public class LocalStorageServiceTests
{
    private static LocalStorageService StorageService => ServicesManager.Instance.GetService<LocalStorageService>();
    
    [SetUp]
    public void Setup()
    {
        ServicesManager.Instance.ResetAllServices();
    }

    [Test]
    public void DefaultInitializeTest()
    {
        LoadServicesOnStartupManager.Initialize();
        Assert.That(StorageService, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(StorageService.World, Is.Not.Null);
            Assert.That(StorageService.WorldDTO, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(StorageService.GetGameStorage(), Is.Not.Null);
            Assert.That(StorageService.GetListStorage(), Is.Not.Null);
        });
    }

    [Test]
    public void DestroyTest()
    {
        Assert.Throws<Exception>(() => StorageService!.Destroy());
    }

    [Test]
    public void GetGameStorageTest()
    {
        Assert.That(StorageService!.GetGameStorage(), Is.Not.Null);
    }

    [Test]
    public void GetListStorageTest()
    {
        Assert.That(StorageService!.GetListStorage(), Is.Not.Null);
    }

    [Test]
    public void SetPreviewWorldTest()
    {
        var worldDTO = new WorldDTO()
        {
            Id = 0,
            Name = "Default World",
            XLenght = 10,
            YLenght = 10,
            PlayerSpawnRoomId = 0,
            Version = App.Version,
        };
        StorageService!.SetWorld(worldDTO);
        var previewWorldDTO = new WorldDTO()
        {
            Id = 0,
            Name = "Default World",
            XLenght = 20,
            YLenght = 20,
            PlayerSpawnRoomId = 0,
            Version = App.Version,
        };
        StorageService!.SetPreviewWorld(previewWorldDTO);
        Assert.That(StorageService.IsPreviewWorldValid, Is.True);
        StorageService.SetPreviewWorld(null);
        Assert.That(StorageService.IsPreviewWorldValid, Is.False);
    }

    [Test]
    public void SetWorldTest()
    {
        var worldDTO = new WorldDTO()
        {
            Id = 0,
            Name = "Default World",
            XLenght = 20,
            YLenght = 20,
            PlayerSpawnRoomId = 0,
            Version = App.Version,
        };
        StorageService!.SetWorld(worldDTO);
        Assert.Multiple(() =>
        {
            Assert.That(StorageService.World, Is.Not.Null);
            Assert.That(StorageService.WorldDTO, Is.Not.Null);
        });
    }
}