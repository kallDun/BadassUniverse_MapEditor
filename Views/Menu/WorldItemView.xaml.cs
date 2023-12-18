using System;
using System.Windows.Controls;
using MapEditor.Models.Server;
using MapEditor.Services;
using MapEditor.Services.Manager;

namespace BadassUniverse_MapEditor.Views.Menu;

public partial class WorldItemView : UserControl
{
    private static LocalStorageService StorageService
        => ServicesManager.Instance.GetService<LocalStorageService>();
    
    public WorldItemView(WorldDTO world)
    {
        InitializeComponent();
        WorldNameTextBlock.Text = world.Name;
        WorldIdTextBlock.Text = $"Id = {world.Id}";
        MainButton.Click += (sender, args) =>
        {
            try
            {
                StorageService.SetWorld(world);
            }
            catch (Exception)
            {
                // ignored
            }
        };
    }
}