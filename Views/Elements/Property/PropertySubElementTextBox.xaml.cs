using System.Windows.Controls;
using MapEditor.Services.Properties.Data;

namespace MapEditor.Views.Elements
{
    public partial class PropertySubElementTextBox : UserControl
    {
        public PropertySubElementTextBox(PropertyData propertyData)
        {
            InitializeComponent();
            NameTextBlock.Text = propertyData.VisualizedName;
            ValueTextBox.Text = propertyData.Value?.ToString();
            ValueTextBox.IsReadOnly = propertyData.IsReadOnly;
            MainGrid.Visibility = propertyData.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            
            propertyData.OnVisibilityChanged += () =>
            {
                MainGrid.Visibility = propertyData.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            };
            
            propertyData.OnValueChangedFromWorld += () =>
            {
                if (ValueTextBox.Text == propertyData.Value?.ToString()) return;
                ValueTextBox.Text = propertyData.Value?.ToString();
            };
            
            ValueTextBox.TextChanged += (sender, args) =>
            {
                if (ValueTextBox.Text == propertyData.Value?.ToString()) return;
                object? returnedValue = propertyData.Value switch
                {
                    string => ValueTextBox.Text,
                    int => int.TryParse(ValueTextBox.Text, out var intValue) ? intValue : propertyData.Value,
                    double => double.TryParse(ValueTextBox.Text, out var doubleValue) ? doubleValue : propertyData.Value,
                    float => float.TryParse(ValueTextBox.Text, out var floatValue) ? floatValue : propertyData.Value,
                    _ => propertyData.Value
                };
                ValueTextBox.Text = returnedValue?.ToString();
                propertyData.SetValue(returnedValue);
            };
        }
    }
}
