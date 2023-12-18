using System.Windows;
using System.Windows.Media;
using MapEditor.Models.Server;
using MapEditor.Services;
using MapEditor.Services.Manager;
using Newtonsoft.Json;

namespace MapEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var worldDto = ServicesManager.Instance.GetService<LocalStorageService>().WorldDTO.Clone() as WorldDTO;
            var str = JsonConvert.SerializeObject(worldDto);
            Clipboard.SetText(str);
        }
    }
}
