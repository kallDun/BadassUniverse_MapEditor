using System.Collections.Generic;
using System.Linq;
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
            if (propertyData.ItemListProperties == null) return;
            for (int i = 0; i < propertyData.ItemListProperties.Count; i++)
            {
                var propertyItem = propertyData.ItemListProperties[i];
                var propertySubElement = PropertyExtensions.GetPropertyView(propertyItem);
                if (propertySubElement != null)
                {
                    var index = i;
                    var view = new PropertySubElementListItem(propertySubElement, () =>
                    {
                        propertyData.RemoveItemFromList(index);
                        UpdateListView(propertyData);
                    });
                    SubElementsStackContainer.Children.Add(view);
                }
            }
            CountSubElementText.Text = propertyData.ItemListProperties.Count.ToString();
        }
        
    }
}
