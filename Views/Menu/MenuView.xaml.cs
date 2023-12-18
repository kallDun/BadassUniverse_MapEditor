using System;
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
        InitServerUrlChanger();
        InitServerLogin();
        InitServerRepository();
    }

    private void InitServerRepository()
    {
        SaveWorldMenuItemButton.Click += async (sender, args) =>
        {
            await ApiConnectionService.TryToAddCurrentWorld();
        };
        LoadWorldMenuItemButton.Click += async (sender, args) =>
        {
            var worlds = await ApiConnectionService.GetWorlds();
            LoadWorldMenuItemButton.Items.Clear();
            foreach (var world in worlds)
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
        };
    }

    private void InitServerLogin()
    {
        LoginMenuItemButton.Click += async (sender, args) =>
        {
            var username = UsernameLoginTextBox.Text;
            var password = PasswordLoginPasswordBox.Password;
            SetTextBlockStatus(LoginStatusTextBlock, "Checking...", Colors.Black);
            LoginMenuItemButton.IsEnabled = false;
            var loginResult = await ApiConnectionService.Login(username, password);
            SetTextBlockStatus(LoginStatusTextBlock, loginResult ? "OK" : "Error", loginResult ? Colors.Green : Colors.Red);
            LoginMenuItemButton.IsEnabled = true;
        };
    }

    private void InitServerUrlChanger()
    {
        SaveServerUrlMenuButton.Click += async (sender, args) =>
        {
            var serverUrl = ServerUrlTextBox.Text;
            ApiConnectionService.SetBaseUrl(serverUrl);
            SetTextBlockStatus(ServerUrlStatusTextBlock, "Checking...", Colors.Black);
            SaveServerUrlMenuButton.IsEnabled = false;
            bool status = await ApiConnectionService.CheckBaseUrl();
            SetTextBlockStatus(ServerUrlStatusTextBlock, status ? "OK" : "Error", status ? Colors.Green : Colors.Red);
            SaveServerUrlMenuButton.IsEnabled = true;
        };
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