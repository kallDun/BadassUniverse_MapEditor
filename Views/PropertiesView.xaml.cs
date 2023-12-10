using System.Windows.Controls;
using BadassUniverse_MapEditor.Views.Elements.Property;
using MapEditor.Services.Manager;
using MapEditor.Services.Properties;

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

            foreach (var property in PropertiesService.Properties)
            {
                var propertySubElement = PropertyExtensions.GetPropertyView(property);
                if (propertySubElement != null) PropertiesPanel.Children.Add(propertySubElement);
            }
        }
    }
}
