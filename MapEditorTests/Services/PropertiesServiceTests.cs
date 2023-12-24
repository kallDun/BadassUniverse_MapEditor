using MapEditor.Services;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties;

namespace MapEditorTests.Services;

public class PropertiesServiceTests
{
    private static LocalStorageService StorageService => ServicesManager.Instance.GetService<LocalStorageService>();
    private static ItemsListService ItemsListService => ServicesManager.Instance.GetService<ItemsListService>();
    private static PropertiesService PropertiesService => ServicesManager.Instance.GetService<PropertiesService>();
    private static PreviewService PreviewService => ServicesManager.Instance.GetService<PreviewService>();
    
    [SetUp]
    public void Setup()
    {
        ServicesManager.Instance.ResetAllServices();
    }
    
    [Test]
    public void DefaultInitializeTest()
    {
        LoadServicesOnStartupManager.Initialize();
        Assert.Multiple(() =>
        {
            Assert.That(StorageService, Is.Not.Null);
            Assert.That(ItemsListService, Is.Not.Null);
            Assert.That(PropertiesService, Is.Not.Null);
            Assert.That(PreviewService, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(PropertiesService.Properties, Is.Not.Null);
            Assert.That(PropertiesService.Properties, Is.Empty);
        });
    }
    
    [Test]
    public void PropertiesWithPreviewTest()
    {
        LoadServicesOnStartupManager.Initialize();
        Assert.Multiple(() =>
        {
            Assert.That(PropertiesService.Properties, Is.Not.Null);
            Assert.That(PropertiesService.Properties, Is.Empty);
        });
        
        var previewItem = ItemsListService.LoadItems().First();
        PreviewService.SetPreviewItem(previewItem.Item, previewItem.Type);
        
        Assert.Multiple(() =>
        {
            Assert.That(PropertiesService.Properties, Is.Not.Null);
            Assert.That(PropertiesService.Properties, Is.Not.Empty);
        });
        
        PreviewService.TryToCancel();
        
        Assert.Multiple(() =>
        {
            Assert.That(PropertiesService.Properties, Is.Not.Null);
            Assert.That(PropertiesService.Properties, Is.Empty);
        });
    }
    
    [Test]
    public void LoadPropertiesToWorldTest()
    {
        LoadServicesOnStartupManager.Initialize();
        Assert.Multiple(() =>
        {
            Assert.That(PropertiesService.Properties, Is.Not.Null);
            Assert.That(PropertiesService.Properties, Is.Empty);
        });
        
        PropertiesService.SetActiveItem(StorageService.WorldDTO);
        
        Assert.Multiple(() =>
        {
            Assert.That(PropertiesService.Properties, Is.Not.Null);
            Assert.That(PropertiesService.Properties, Is.Not.Empty);
        });
    }
}