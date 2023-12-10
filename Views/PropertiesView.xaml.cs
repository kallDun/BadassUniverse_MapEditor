using System;
using System.Windows.Controls;
using System.Windows.Media;
using BadassUniverse_MapEditor.Views.Elements.Property;
using MapEditor.Models.Server;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties;
using MapEditor.Views.Elements;

namespace MapEditor.Views
{
    public partial class PropertiesView : UserControl
    {
        private static PropertiesService PropertiesService
            => ServicesManager.Instance.GetService<PropertiesService>();
        
        public PropertiesView()
        {
            InitializeComponent();
            PropertiesService.OnPropertiesChanged += UpdatePropertiesView;
        }
        
        private void UpdatePropertiesView()
        {
            PropertiesPanel.Children.Clear();

            foreach (var property in PropertiesService.Properties)
            {
                UserControl? propertySubElement = property.Value switch
                {
                    string or int or double or float => new PropertySubElementTextBox(property),
                    bool => new PropertySubElementCheckBox(property),
                    Enum => new PropertySubElementComboBox(property),
                    Color or ColorDTO => new PropertySubElementColorPicker(property),
                    null => null,
                    _ => property.SubProperties?.Count > 0 
                        ? new PropertySubElementItem(property) 
                        : null, 
                };
                if (propertySubElement != null)
                {
                    PropertiesPanel.Children.Add(propertySubElement);
                }
            }
        }
    }
}
