using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MapEditor.Services.Properties.Data;

namespace BadassUniverse_MapEditor.Views.Elements.Property
{
    public partial class PropertySubElementList : UserControl
    {
        public PropertySubElementList(PropertyData propertyData)
        {
            InitializeComponent();
            if (propertyData.Value is not IEnumerable<object> 
                || propertyData.ItemListProperties is null) return;
            
            NameTextBlock.Text = propertyData.VisualizedName;
            BorderMain.Visibility = propertyData.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            propertyData.OnVisibilityChanged += () =>
            {
                BorderMain.Visibility = propertyData.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            };
            
            foreach (var propertyItem in propertyData.ItemListProperties)
            {
                var propertySubElement = PropertyExtensions.GetPropertyView(propertyItem);
                if (propertySubElement != null) SubElementsStackContainer.Children.Add(propertySubElement);
            }
        }
    }
}
