namespace MapEditorTests.Services;

public class ItemsListServiceTests
{
    private static ItemsListService ItemsListService => ServicesManager.Instance.GetService<ItemsListService>();
    
    [SetUp]
    public void Setup()
    {
        ServicesManager.Instance.ResetAllServices();
    }
    
    [Test]
    public void DefaultInitializeTest()
    {
        LoadServicesOnStartupManager.Initialize();
        Assert.That(ItemsListService, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(ItemsListService.LoadItems(), Is.Not.Null);
            Assert.That(ItemsListService.LoadItems(), Is.Not.Empty);
        });
    }
}