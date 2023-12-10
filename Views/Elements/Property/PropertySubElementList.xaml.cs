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
            UpdateListView(propertyData);
            
            AddSubElementButton.Click += (sender, args) =>
            {
                propertyData.AddItemToList();
                UpdateListView(propertyData);
            };
            RemoveSubElementButton.Click += (sender, args) =>
            {
                int lastItemIndex = propertyData.ItemListProperties.Count - 1;
                if (lastItemIndex < 0) return;
                propertyData.RemoveItemFromList(lastItemIndex);
                UpdateListView(propertyData);
            };
        }

        private void UpdateListView(PropertyData propertyData)
        {
            SubElementsStackContainer.Children.Clear();
            foreach (var propertyItem in propertyData.ItemListProperties)
            {
                var propertySubElement = PropertyExtensions.GetPropertyView(propertyItem);
                if (propertySubElement != null) SubElementsStackContainer.Children.Add(propertySubElement);
            }
        }
    }
}
