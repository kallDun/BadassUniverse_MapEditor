using System;
using System.Windows.Controls;
using MapEditor.Services.Properties.Data;

namespace MapEditor.Views.Elements
{
    public partial class PropertySubElementCheckBox : UserControl
    {
        public PropertySubElementCheckBox(PropertyData propertyData)
        {
            InitializeComponent();
            if (propertyData.Value is not bool value) throw 
                new ArgumentException("PropertySubElementCheckBox can only be used with bool properties.");
            
            NameTextBlock.Text = propertyData.VisualizedName;
            ValueCheckBox.IsEnabled = !propertyData.IsReadOnly;
            ValueCheckBox.IsChecked = value;

            DirtyFlagCheckBox.Visibility = propertyData.DirtyFlag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            propertyData.OnDirtyFlagChanged += () =>
            {
                DirtyFlagCheckBox.Visibility = propertyData.DirtyFlag ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            };

            MainGrid.Visibility = propertyData.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            propertyData.OnVisibilityChanged += () =>
            {
                MainGrid.Visibility = propertyData.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            };
            
            propertyData.OnValueChangedFromWorld += () =>
            {
                if (ValueCheckBox.IsChecked == propertyData.Value as bool?) return;
                ValueCheckBox.IsChecked = propertyData.Value as bool?;
                ValueCheckBox.Content = ValueCheckBox.IsChecked is true ? "Yes" : "No";
            };
            
            ValueCheckBox.Checked += (sender, args) =>
            {
                if (ValueCheckBox.IsChecked == propertyData.Value as bool?) return;
                ValueCheckBox.Content = ValueCheckBox.IsChecked is true ? "Yes" : "No";
                propertyData.SetValue(ValueCheckBox.IsChecked);
            };
            ValueCheckBox.Unchecked += (sender, args) =>
            {
                if (ValueCheckBox.IsChecked == propertyData.Value as bool?) return;
                ValueCheckBox.Content = ValueCheckBox.IsChecked is true ? "Yes" : "No";
                propertyData.SetValue(ValueCheckBox.IsChecked);
            };
        }
    }
}
