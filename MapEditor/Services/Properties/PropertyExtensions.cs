using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using MapEditor.Models.Server;
using MapEditor.Services.Properties.Data;
using MapEditor.Views.Elements;

namespace MapEditor.Views.Elements.Property;

public static class PropertyExtensions
{
    
    public static UserControl? GetPropertyView(PropertyData property)
    {
        UserControl? propertySubElement = property.Value switch
        {
            string or int or double or float => new PropertySubElementTextBox(property),
            bool => new PropertySubElementCheckBox(property),
            Enum => new PropertySubElementComboBox(property),
            Color or ColorDTO => new PropertySubElementColorPicker(property),
            IList or Array => new PropertySubElementList(property),
            null => null,
            _ => property.SubProperties?.Count > 0
                ? new PropertySubElementItem(property)
                : null,
        };
        return propertySubElement;
    }
    
}