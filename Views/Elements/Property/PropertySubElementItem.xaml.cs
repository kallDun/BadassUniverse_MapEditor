using System.Windows;
using System.Windows.Controls;
using MapEditor.Services.Properties.Data;

namespace MapEditor.Views.Elements.Property
{
    public partial class PropertySubElementItem : UserControl
    {
        public PropertySubElementItem(PropertyData propertyData)
        {
            InitializeComponent();
            if (propertyData.SubProperties == null) return;
            
            NameTextBlock.Text = propertyData.VisualizedName;

            DirtyFlagItem.Visibility = propertyData.DirtyFlag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            propertyData.OnDirtyFlagChanged += () =>
            {
                DirtyFlagItem.Visibility = propertyData.DirtyFlag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            };

            BorderMain.Visibility = propertyData.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            propertyData.OnVisibilityChanged += () =>
            {
                BorderMain.Visibility = propertyData.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            };
            
            foreach (var property in propertyData.SubProperties)
            {
                UserControl? propertySubElement = PropertyExtensions.GetPropertyView(property);
                if (propertySubElement != null) MainStackContainer.Children.Add(propertySubElement);
            }
        }
    }
}
