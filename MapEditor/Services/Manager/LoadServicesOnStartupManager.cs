namespace MapEditor.Services.Manager
{
    public static class LoadServicesOnStartupManager
    {
        public static void Initialize()
        {
            ServicesManager.Instance.RegisterService<LocalStorageService>();
            ServicesManager.Instance.RegisterService<InitializationDefaultWorldService>();
        }
    }
}
