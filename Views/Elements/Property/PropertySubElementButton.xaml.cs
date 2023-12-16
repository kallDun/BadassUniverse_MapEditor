using System.Windows.Controls;
using MapEditor.Services.Properties.Data;

namespace MapEditor.Views.Elements
{
    public partial class PropertySubElementButton : UserControl
    {
        public PropertySubElementButton(ActionButtonData data)
        {
            InitializeComponent();
            NameTextBlock.Text = data.Name;
            ActionButton.Content = data.ButtonName;
            ActionButton.Click += (sender, e) => data.OnClick();
            ActionButton.IsEnabled = data.IsEnabled;
            data.OnEnabledChanged += () => ActionButton.IsEnabled = data.IsEnabled;
        }
    }
}
