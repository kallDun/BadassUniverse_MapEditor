using System.Windows;
using MapEditor.Services.Manager;

namespace MapEditor
{
    public partial class App : Application
    {
        public readonly string Version = "0.0.1";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            LoadServicesOnStartupManager.Initialize();
        }
    }
}
