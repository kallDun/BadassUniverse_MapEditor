using System;
using System.Windows.Controls;
using System.Windows.Media;
using MapEditor.Models.Server;
using MapEditor.Services.Properties.Data;

namespace MapEditor.Views.Elements
{
    public partial class PropertySubElementColorPicker : UserControl
    {
        public PropertySubElementColorPicker(PropertyData propertyData)
        {
            InitializeComponent();
            if (propertyData.Value is not (Color or ColorDTO)) throw new ArgumentException("PropertyData.Value is not Color");
            var value = propertyData.Value is Color color ? color : (Color)(propertyData.Value is ColorDTO dto ? dto : default);
            
            NameTextBlock.Text = propertyData.VisualizedName;
            ValueColorPicker.IsEnabled = !propertyData.IsReadOnly;
            ValueColorPicker.Color = value;

            DirtyFlagColorPicker.Visibility = propertyData.DirtyFlag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            propertyData.OnDirtyFlagChanged += () =>
            {
                DirtyFlagColorPicker.Visibility = propertyData.DirtyFlag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            };

            MainGrid.Visibility = propertyData.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            propertyData.OnVisibilityChanged += () =>
            {
                MainGrid.Visibility = propertyData.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            };
            
            propertyData.OnValueChangedFromWorld += () =>
            {
                var color = propertyData.Value is Color clr ? clr : (Color)(propertyData.Value is ColorDTO dto ? dto : default);
                if (ValueColorPicker.Color == color) return;
                ValueColorPicker.Color = color;
            };
            
            ValueColorPicker.ColorChanged += (sender, args) =>
            {
                var color = propertyData.Value is Color clr ? clr : (Color)(propertyData.Value is ColorDTO dto ? dto : default);
                if (ValueColorPicker.Color == color) return;
                propertyData.SetValue(ValueColorPicker.Color);
            };
        }
    }
}
