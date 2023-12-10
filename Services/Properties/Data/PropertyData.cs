using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapEditor.Models.Server;
using MapEditor.Services.Properties.Attributes;

namespace MapEditor.Services.Properties.Data;

public class PropertyData
{
    private readonly AItemDTO item;
    private readonly PropertyInfo property;
    private readonly CustomPropertyAttribute attribute;
    
    public string VisualizedName { get; private set; }
    public bool IsReadOnly { get; private set; }
    public bool IsVisible { get; private set; }
    public object? Value { get; private set; }
    public Action? OnValueChangedFromWorld { get; set; }
    public Action? OnValueChangedFromProperties { get; set; }
    public Action? OnVisibilityChanged { get; set; }
    
    public PropertyData(AItemDTO item, PropertyInfo property, CustomPropertyAttribute attribute)
    {
        this.item = item;
        this.property = property;
        this.attribute = attribute;

        VisualizedName = string.IsNullOrEmpty(attribute.PropertyName) ? property.Name : attribute.PropertyName;
        IsReadOnly = attribute.IsReadOnly;
        IsVisible = true;
        Value = property.GetValue(item);

        item.OnValueChanged += (propertyName) =>
        {
            if (propertyName != property.Name) return;
            Value = property.GetValue(item);
            OnValueChangedFromWorld?.Invoke();
        };
    }
    
    public void InitRelations(IEnumerable<PropertyData> properties)
    {
        if (attribute.ShowIfProperty == null) return;
        var relatedProperty = properties.FirstOrDefault(x => x.property.Name == attribute.ShowIfProperty);
        if (relatedProperty == null) return;
        ChangePropertyVisibility(relatedProperty);
        relatedProperty.OnValueChangedFromProperties += () => ChangePropertyVisibility(relatedProperty);
        relatedProperty.OnValueChangedFromWorld += () => ChangePropertyVisibility(relatedProperty);
    }
    
    private void ChangePropertyVisibility(PropertyData relatedProperty)
    {
        IsVisible = relatedProperty.Value?.Equals(attribute.ShowIfValue) ?? relatedProperty.Value == attribute.ShowIfValue;
        OnVisibilityChanged?.Invoke();
    }

    public void SetValue(object? value)
    {
        if (value == null) return;
        property.SetValue(item, value);
        Value = value;
        OnValueChangedFromProperties?.Invoke();
    }
}