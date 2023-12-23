using System;
using System.Windows;
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
            ValueComboBox.IsReadOnly = propertyData.IsReadOnly;
            ValueComboBox.IsEditable = !propertyData.IsReadOnly;
            ValueComboBox.IsEnabled = !propertyData.IsReadOnly;
            string[] names = Enum.GetNames(propertyData.Value.GetType());
            ValueComboBox.ItemsSource = names;
            ValueComboBox.SelectedIndex = Array.IndexOf(names, propertyData.Value.ToString());

            DirtyFlagComboBox.Visibility = propertyData.DirtyFlag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            propertyData.OnDirtyFlagChanged += () =>
            {
                DirtyFlagComboBox.Visibility = propertyData.DirtyFlag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            };

            MainGrid.Visibility = propertyData.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            propertyData.OnVisibilityChanged += () =>
            {
                MainGrid.Visibility = propertyData.IsVisible ? Visibility.Visible : Visibility.Collapsed;
            };
            
            propertyData.OnValueChangedFromWorld += () =>
            {
                if (ValueComboBox.SelectedItem.ToString() == propertyData.Value.ToString()) return;
                ValueComboBox.SelectedIndex = Array.IndexOf(names, propertyData.Value.ToString());
            };

            ValueComboBox.SelectionChanged += (sender, args) =>
            {
                if (ValueComboBox.SelectedItem.ToString() == propertyData.Value.ToString()) return;
                propertyData.SetValue(Enum.Parse(propertyData.Value.GetType(), (string)ValueComboBox.SelectedItem));
            };
        }
    }
}
