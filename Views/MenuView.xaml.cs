using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MapEditor.Models.Server;
using MapEditor.Services;
using MapEditor.Services.Manager;
using Newtonsoft.Json;

namespace BadassUniverse_MapEditor.Views.Menu;

public partial class MenuView : UserControl
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    private static ApiConnectorService ApiConnectionService 
        => ServicesManager.Instance.GetService<ApiConnectorService>();
    
    public MenuView()
    {
        InitializeComponent();
        
        SaveServerUrlMenuButton.Click += async (sender, args) => { await TryToSaveServerUrl(); };
        LoginMenuItemButton.Click += async (sender, args) => { await TryToLogin(); };
        SaveWorldMenuItemButton.Click += async (sender, args) => { await TryToSaveWorld(); };
        LoadWorldMenuItemButton.Click += async (sender, args) => { await LoadWorlds(); };
        _ = TryToSaveServerUrl();
    }

    private async Task LoadWorlds()
    {
        SaveWorldMenuItemButton.IsEnabled = false;
        LoadWorldMenuItemButton.IsEnabled = false;
        SetTextBlockStatus(RepositoryStatusTextBlock, "Loading...", Colors.Black);
        
        WorldsListMenuItem.IsEnabled = false;
        WorldsListMenuItem.Items.Clear();
        var worlds = await ApiConnectionService.GetWorlds();
        var worldArray = worlds as WorldDTO[] ?? worlds.ToArray();
        foreach (var world in worldArray)
        {
            var item = new MenuItem
            {
                Header = world.Name,
                Tag = world
            };
            item.Click += (o, eventArgs) =>
            {
                var worldDto = (WorldDTO) item.Tag;
                StorageService.SetWorld(worldDto);
            };
            LoadWorldMenuItemButton.Items.Add(item);
        }
        WorldsListMenuItem.IsEnabled = worldArray.Any();
        
        SetTextBlockStatus(RepositoryStatusTextBlock, worldArray.Any() ? "OK" : "Error", worldArray.Any() ? Colors.Green : Colors.Red);
        SaveWorldMenuItemButton.IsEnabled = true;
        LoadWorldMenuItemButton.IsEnabled = true;
    }
    
    private async Task TryToSaveWorld()
    {
        SaveWorldMenuItemButton.IsEnabled = false;
        LoadWorldMenuItemButton.IsEnabled = false;
        SetTextBlockStatus(RepositoryStatusTextBlock, "Saving...", Colors.Black);
        
        bool result = await ApiConnectionService.TryToAddCurrentWorld();
        
        SetTextBlockStatus(RepositoryStatusTextBlock, result ? "OK" : "Error", result ? Colors.Green : Colors.Red);
        SaveWorldMenuItemButton.IsEnabled = true;
        LoadWorldMenuItemButton.IsEnabled = true;
    }

    private async Task TryToLogin()
    {
        var username = UsernameLoginTextBox.Text;
        var password = PasswordLoginPasswordBox.Password;
        SetTextBlockStatus(LoginStatusTextBlock, "Checking...", Colors.Black);
        LoginMenuItemButton.IsEnabled = false;
        var loginResult = await ApiConnectionService.Login(username, password);
        SetTextBlockStatus(LoginStatusTextBlock, loginResult ? "OK" : "Error", loginResult ? Colors.Green : Colors.Red);
        LoginMenuItemButton.IsEnabled = true;
    }

    private async Task TryToSaveServerUrl()
    {
        var serverUrl = ServerUrlTextBox.Text;
        ApiConnectionService.SetBaseUrl(serverUrl);
        SetTextBlockStatus(ServerUrlStatusTextBlock, "Checking...", Colors.Black);
        SaveServerUrlMenuButton.IsEnabled = false;
        bool status = await ApiConnectionService.CheckBaseUrl();
        SetTextBlockStatus(ServerUrlStatusTextBlock, status ? "OK" : "Error", status ? Colors.Green : Colors.Red);
        SaveServerUrlMenuButton.IsEnabled = true;
    }

    private static void SetTextBlockStatus(TextBlock textBlock, string text, Color color)
    {
        textBlock.Text = text;
        textBlock.Foreground = new SolidColorBrush(color);
    }

    private void SaveMenuButton_OnClick(object sender, RoutedEventArgs e)
    {
        var worldDto = StorageService.WorldDTO.Clone() as WorldDTO;
        var str = JsonConvert.SerializeObject(worldDto);
        Clipboard.SetText(str);
    }

    private void OpenMenuButton_OnClick(object sender, RoutedEventArgs e)
    {
        var str = Clipboard.GetText();
        try
        {
            var worldDto = JsonConvert.DeserializeObject<WorldDTO>(str);
            if (worldDto == null) 
                throw new ArgumentException("World is null!");
            StorageService.SetWorld(worldDto);
        }
        catch (Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}