using System.Windows.Controls;
using MapEditor.Views.Elements.Property;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties;
using MapEditor.Views.Elements;

namespace MapEditor.Views
{
    public partial class PropertiesView : UserControl
    {
        private static PropertiesService PropertiesService
            => ServicesManager.Instance.GetService<PropertiesService>();
        
        public PropertiesView()
        {
            InitializeComponent();
            PropertiesService.OnPropertiesChanged += UpdatePropertiesView;
        }
        
        private void UpdatePropertiesView()
        {
            PropertiesPanel.Children.Clear();

            foreach (var actionButton in PropertiesService.ActionButtons)
            {
                PropertiesPanel.Children.Add(new PropertySubElementButton(actionButton));
            }
            foreach (var property in PropertiesService.Properties)
            {
                var propertySubElement = PropertyExtensions.GetPropertyView(property);
                if (propertySubElement != null) PropertiesPanel.Children.Add(propertySubElement);
            }
        }
    }
}
