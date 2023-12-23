using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using MapEditor.Models.Server;
using MapEditor.Services.Properties.Data;
using Xceed.Wpf.Toolkit;

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
            ValueColorPicker.SelectedColor = value;

            ValueColorPicker.StandardColors = new ObservableCollection<ColorItem>()
            {
                new(Colors.Red, "Giga Red"),
                new(Colors.Orange, "Giga Orange"),
                new(Colors.Yellow, "Giga Yellow"),
                new(Colors.Green, "Giga Green"),
                new(Colors.Blue, "Giga Blue"),
            };

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
                if (ValueColorPicker.SelectedColor == color) return;
                ValueColorPicker.SelectedColor = color;
            };

            ValueColorPicker.SelectedColorChanged += (sender, args) =>
            {
                var color = propertyData.Value is Color clr ? clr : (Color)(propertyData.Value is ColorDTO dto ? dto : default);
                if (ValueColorPicker.SelectedColor == color) return;
                if (propertyData.Value is ColorDTO)
                {
                    var colorDto = new ColorDTO();
                    colorDto.Red = ValueColorPicker.SelectedColor.Value.R;
                    colorDto.Green = ValueColorPicker.SelectedColor.Value.G;
                    colorDto.Blue = ValueColorPicker.SelectedColor.Value.B;
                    propertyData.SetValue(colorDto);
                }
            };
        }
    }
}
