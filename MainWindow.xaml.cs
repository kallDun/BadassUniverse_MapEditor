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
            
            worldDto.Facades.Add(new FacadeDTO()
            {
                Name = "Test Facade",
            });
            worldDto.Rooms[0].Mobs.Add(new MobDTO()
            {
                Name = "Test Mob",
            });
            worldDto.Rooms[1].PhysicsItems.Add(new PhysicsItemDTO()
            {
                Name = "Test Item",
            });
            
            var str = JsonConvert.SerializeObject(worldDto);
            Clipboard.SetText(str);
        }
    }
}
