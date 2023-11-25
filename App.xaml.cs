using BadassUniverse_MapEditor.Services.Manager;
using BadassUniverse_MapEditor.Services;
using System.Windows;

namespace BadassUniverse_MapEditor
{
    public partial class App : Application
    {
        public readonly string Version = "0.0.1";

        private static InitializationDefaultMapService DefaultMapService
            => ServicesManager.Instance.GetService<InitializationDefaultMapService>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DefaultMapService.InitializeDefaultMap();
        }
    }
}
