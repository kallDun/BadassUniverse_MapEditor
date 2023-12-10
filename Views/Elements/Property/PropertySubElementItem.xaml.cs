using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MapEditor.Models.Server;
using MapEditor.Services.Properties.Data;
using MapEditor.Views.Elements;

namespace BadassUniverse_MapEditor.Views.Elements.Property
{
    public partial class PropertySubElementItem : UserControl
    {
        public PropertySubElementItem(PropertyData propertyData)
        {
            InitializeComponent();
            if (propertyData.SubProperties == null) return;
            
            NameTextBlock.Text = propertyData.VisualizedName;
            BorderMain.Visibility = propertyData.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            propertyData.OnVisibilityChanged += () =>
            {
                BorderMain.Visibility = propertyData.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            };
            
            foreach (var property in propertyData.SubProperties)
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
                    MainStackContainer.Children.Add(propertySubElement);
                }
            }
        }
    }
}
