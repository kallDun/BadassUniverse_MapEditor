using System;
using System.Windows;
using System.Windows.Controls;

namespace BadassUniverse_MapEditor.Views.Elements.Property
{
    public partial class PropertySubElementListItem : UserControl
    {
        public PropertySubElementListItem(UIElement userControl, Action removeAction)
        {
            InitializeComponent();
            MainGrid.Children.Add(userControl);
            RemoveButton.Click += (sender, args) => removeAction();
        }
    }
}
