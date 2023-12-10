using System;
using System.Windows.Controls;
using MapEditor.Services.Properties.Data;

namespace MapEditor.Views.Elements
{
    public partial class PropertySubElementComboBox : UserControl
    {
        public PropertySubElementComboBox(PropertyData propertyData)
        {
            InitializeComponent();
            if (propertyData.Value == null) return;
            
            NameTextBlock.Text = propertyData.VisualizedName;
            string[] names = Enum.GetNames(propertyData.Value.GetType());
            ValueComboBox.ItemsSource = names;
            ValueComboBox.SelectedIndex = Array.IndexOf(names, propertyData.Value.ToString());

            ValueComboBox.SelectionChanged += (sender, args) =>
            {
                propertyData.SetValue(Enum.Parse(propertyData.Value.GetType(), (string)ValueComboBox.SelectedItem));
            };
        }
    }
}
